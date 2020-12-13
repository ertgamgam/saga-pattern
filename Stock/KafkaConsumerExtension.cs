using KafkaBroker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stock.Consumer.OrderCreated;

namespace Stock
{
    public static class KafkaConsumerExtension
    {
        public static IServiceCollection AddConsumers(this IServiceCollection services)
        {
            services.AddSingleton<OrderCreatedHandler>();
            var serviceProvider = services.BuildServiceProvider();

            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var orderCreatedHandler = serviceProvider.GetRequiredService<OrderCreatedHandler>();
            
            services.AddHostedService((provider => new OrderCreatedBackgroundService(
                orderCreatedHandler, new KafkaConsumerConfiguration
                {
                    KafkaHost = configuration.GetValue<string>("KafkaHost"),
                    TopicName = "order-created",
                    GroupId = "stock-service"
                })));

            return services;
        }
    }
}