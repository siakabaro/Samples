
using WebSocketsServer.Services;

namespace WebSocketsServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddResourceMonitoring();

            //Register connection manager and messages handlers
            builder.Services.AddSingleton<IConnectionManager, ConnectionManager>();
            builder.Services.AddTransient<IMessagesHandler, MessagesHandler>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();
            app.UseAuthorization();

            var webSocketOptions = new WebSocketOptions
            {
                KeepAliveInterval = TimeSpan.FromMinutes(1)
            };
            //Please add the allowed origins here
            //webSocketOptions.AllowedOrigins.Add("https://siakabaro.com");
            app.UseWebSockets(webSocketOptions);

            app.MapControllers();

            app.Run();
        }
    }
}
