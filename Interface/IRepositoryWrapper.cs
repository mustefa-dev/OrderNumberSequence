namespace OrderNumberSequence.Interface;

public interface IRepositoryWrapper
{
    IUserRepository User { get; }

    // here to add
IOrderProductRepository OrderProduct{get;}
IOrderRepository Order{get;}
IProductRepository Product{get;}
IMessageRepository Message{get;}

}
