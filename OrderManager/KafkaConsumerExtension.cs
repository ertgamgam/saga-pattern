using KafkaBroker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderManager.Consumer.OrderStockError;

namespace OrderManager
{
    public static class KafkaConsumerExtension
    {
        private const string OrderStockUpdateErrorTopicName = "order-stock-update-error";
        private const string GroupId = "order-manager";

        public static IServiceCollection AddConsumers(this IServiceCollection services)
        {
            services.AddScoped<OrderStockErrorHandler>();
            var serviceProvider = services.BuildServiceProvider();

            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var orderStockErrorHandler = serviceProvider.GetRequiredService<OrderStockErrorHandler>();

            services.AddHostedService((provider => new OrderStockErrorBackgroundService(
                orderStockErrorHandler, new KafkaConsumerConfiguration
                {
                    KafkaHost = configuration.GetValue<string>("KafkaHost"),
                    TopicName = OrderStockUpdateErrorTopicName,
                    GroupId = GroupId
                })));

            return services;
        }
    }
}