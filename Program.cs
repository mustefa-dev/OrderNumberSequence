using System;
using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerUI;
using OrderNumberSequence.Extensions;
using OrderNumberSequence.Helpers;
using ConfigurationProvider = OrderNumberSequence.Helpers.ConfigurationProvider; 

var builder = WebApplication.CreateBuilder(args);

// Add SignalR services
builder.Services.AddSignalR();

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Error()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        builder => builder
            .WithOrigins("http://localhost:63342") // Specify the exact origin
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()); // Allow credentials
});

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
        options.SerializerSettings.Converters.Add(new IsoDateTimeConverter
            { DateTimeStyles = DateTimeStyles.AssumeUniversal });
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => { options.OperationFilter<PascalCaseQueryParameterFilter>(); });
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);

IConfiguration configuration = builder.Configuration;
ConfigurationProvider.Configuration = configuration;
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    options.RoutePrefix = string.Empty; // Serve Swagger UI at the app's root
    options.DocExpansion(DocExpansion.None);
});
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("AllowSpecificOrigins"); 
app.UseMiddleware<CustomUnauthorizedMiddleware>();
app.UseMiddleware<CustomPayloadTooLargeMiddleware>();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<ChatHub>("/chatHub");
});
// app.UseCors("AllowSpecificOrigins"); // Remove this line
app.MapControllers();

app.Run();

app.Run();
