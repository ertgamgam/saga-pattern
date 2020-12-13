using KafkaBroker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stock.Consumer.OrderCreated;
using Stock.Consumer.OrderWalletPayError;

namespace Stock
{
    public static class KafkaConsumerExtension
    {
        private const string OrderCreatedTopicName = "order-created";
        private const string OrderWalletPayErrorTopicName = "order-wallet-pay-error";

        private const string GroupId = "stock-service";

        public static IServiceCollection AddConsumers(this IServiceCollection services)
        {
            services.AddScoped<OrderCreatedHandler>()
                .AddScoped<OrderWalletPayErrorHandler>();
            var serviceProvider = services.BuildServiceProvider();

            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var orderCreatedHandler = serviceProvider.GetRequiredService<OrderCreatedHandler>();
            var orderWalletPayErrorHandler = serviceProvider.GetRequiredService<OrderWalletPayErrorHandler>();

            services.AddHostedService((provider => new OrderCreatedBackgroundService(
                orderCreatedHandler, new KafkaConsumerConfiguration
                {
                    KafkaHost = configuration.GetValue<string>("KafkaHost"),
                    TopicName = OrderCreatedTopicName,
                    GroupId = GroupId
                })));

            services.AddHostedService((provider => new OrderWalletPayErrorBackgroundService(
                orderWalletPayErrorHandler, new KafkaConsumerConfiguration
                {
                    KafkaHost = configuration.GetValue<string>("KafkaHost"),
                    TopicName = OrderWalletPayErrorTopicName,
                    GroupId = GroupId
                })));


            return services;
        }
    }
}