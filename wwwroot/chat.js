let connection = new signalR.HubConnectionBuilder()
    .withUrl("http://localhost:5272/chatHub") // Update this with the correct URL of your SignalR hub
    .build();

// Define the function that the server can call to push messages
connection.on("ReceiveMessage", function(fromUser, message) {
    console.log('Received message from ' + fromUser + ': ' + message);
    let messages = document.getElementById("messages");
    let messageItem = document.createElement("div");
    messageItem.textContent = fromUser + ": " + message;
    messages.appendChild(messageItem);
    window.scrollTo(0, document.body.scrollHeight); // Auto scroll to the bottom
});

// Start the connection
startSignalRConnection(connection);

// Function to send messages
function sendMessage() {
    let userInput = document.getElementById("userInput");
    let messageInput = document.getElementById("messageInput");
    let fromUser = userInput.value;
    let message = messageInput.value;
    console.log('Sending message from ' + fromUser + ': ' + message);
    if (connection.state === signalR.HubConnectionState.Connected) {
        connection.invoke("SendMessage", fromUser, message)
            .catch(function (err) {
                return console.error(err.toString());
            });
        messageInput.value = '';
    } else {
        console.error('Cannot send message, connection is not in the Connected state');
    }
}

// Add event listener to send button
document.addEventListener('DOMContentLoaded', (event) => {
    let sendButton = document.getElementById("sendButton");
    sendButton.addEventListener("click", function(event) {
        event.preventDefault();
        sendMessage();
    });
});

// Add reconnection logic
connection.onclose(async () => {
    await startSignalRConnection(connection);
});

function displayError(message) {
    let messages = document.getElementById("messages");
    let messageItem = document.createElement("div");
    messageItem.textContent = "Error: " + message;
    messageItem.style.color = "red";
    messages.appendChild(messageItem);
    window.scrollTo(0, document.body.scrollHeight); // Auto scroll to the bottom
}

async function startSignalRConnection(connection) {
    try {
        await connection.start();
        console.log('connected');

        // Call the GetOldMessages method on the server
        connection.invoke("GetOldMessages")
            .then(function (oldMessages) {
                // Iterate over the old messages and display them
                for (let i = 0; i < oldMessages.length; i++) {
                    let message = oldMessages[i];
                    let messages = document.getElementById("messages");
                    let messageItem = document.createElement("div");
                    messageItem.textContent = message.fromUser + ": " + message.message;
                    messages.appendChild(messageItem);
                }
                window.scrollTo(0, document.body.scrollHeight); // Auto scroll to the bottom
            })
            .catch(function (err) {
                return console.error(err.toString());
            });
    } catch (err) {
        console.log('Error while establishing connection: ' + err);
        displayError('Error while establishing connection: ' + err);
        setTimeout(() => startSignalRConnection(connection), 5000);
    }
}