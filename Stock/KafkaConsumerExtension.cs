using KafkaBroker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stock.Consumer.OrderCreated;
using Stock.Repository;

namespace Stock
{
    public static class KafkaConsumerExtension
    {
        private const string OrderCreatedTopicName = "order-created";
        private const string GroupId = "stock-service";

        public static IServiceCollection AddConsumers(this IServiceCollection services)
        {
            services.AddScoped<OrderCreatedHandler>();
            var serviceProvider = services.BuildServiceProvider();

            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var orderCreatedHandler = serviceProvider.GetRequiredService<OrderCreatedHandler>();

            services.AddHostedService((provider => new OrderCreatedBackgroundService(
                orderCreatedHandler, new KafkaConsumerConfiguration
                {
                    KafkaHost = configuration.GetValue<string>("KafkaHost"),
                    TopicName = OrderCreatedTopicName,
                    GroupId = GroupId
                })));

            return services;
        }
    }
}