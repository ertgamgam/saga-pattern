using KafkaBroker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wallet.Consumer.OrderStockUpdated;

namespace Wallet
{
    public static class KafkaConsumerExtension
    {
        private const string OrderStockUpdatedTopicName = "order-stock-updated";
        private const string GroupId = "wallet-service";

        public static IServiceCollection AddConsumers(this IServiceCollection services)
        {
            services.AddScoped<OrderStockUpdatedHandler>();
            var serviceProvider = services.BuildServiceProvider();

            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var orderCreatedHandler = serviceProvider.GetRequiredService<OrderStockUpdatedHandler>();

            services.AddHostedService((provider => new OrderStockUpdatedBackgroundService(
                orderCreatedHandler, new KafkaConsumerConfiguration
                {
                    KafkaHost = configuration.GetValue<string>("KafkaHost"),
                    TopicName = OrderStockUpdatedTopicName,
                    GroupId = GroupId
                })));

            return services;
        }
    }
}