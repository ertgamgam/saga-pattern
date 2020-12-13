using KafkaBroker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderManager.Consumer.OrderStockError;
using OrderManager.Consumer.OrderWalletPayCompleted;
using OrderManager.Consumer.OrderWalletPayError;

namespace OrderManager
{
    public static class KafkaConsumerExtension
    {
        private const string OrderStockUpdateErrorTopicName = "order-stock-update-error";
        private const string OrderWalletPayErrorTopicName = "order-wallet-pay-error";
        private const string OrderWalletPayCompletedTopicName = "order-wallet-pay-completed";

        private const string GroupId = "order-manager";

        public static IServiceCollection AddConsumers(this IServiceCollection services)
        {
            services.AddScoped<OrderStockErrorHandler>()
                .AddScoped<OrderWalletPayErrorHandler>()
                .AddScoped<OrderWalletPayCompletedHandler>();

            var serviceProvider = services.BuildServiceProvider();

            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var orderStockErrorHandler = serviceProvider.GetRequiredService<OrderStockErrorHandler>();
            var orderWalletPayErrorHandler = serviceProvider.GetRequiredService<OrderWalletPayErrorHandler>();
            var orderWalletPayCompletedHandler = serviceProvider.GetRequiredService<OrderWalletPayCompletedHandler>();

            services.AddHostedService((provider => new OrderStockErrorBackgroundService(
                orderStockErrorHandler, new KafkaConsumerConfiguration
                {
                    KafkaHost = configuration.GetValue<string>("KafkaHost"),
                    TopicName = OrderStockUpdateErrorTopicName,
                    GroupId = GroupId
                })));

            services.AddHostedService((provider => new OrderWalletPayErrorBackgroundService(
                orderWalletPayErrorHandler, new KafkaConsumerConfiguration
                {
                    KafkaHost = configuration.GetValue<string>("KafkaHost"),
                    TopicName = OrderWalletPayErrorTopicName,
                    GroupId = GroupId
                })));

            services.AddHostedService((provider => new OrderWalletPayCompletedBackgroundService(
                orderWalletPayCompletedHandler, new KafkaConsumerConfiguration
                {
                    KafkaHost = configuration.GetValue<string>("KafkaHost"),
                    TopicName = OrderWalletPayCompletedTopicName,
                    GroupId = GroupId
                })));

            return services;
        }
    }
}