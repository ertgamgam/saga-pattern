using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;

namespace KafkaBroker
{
        public abstract class KafkaConsumerBackgroundService<THandler, TMessage> : BackgroundService
        where THandler : IHandler<TMessage>
    {
        private readonly IConsumer<string, string> _consumer;
        private readonly THandler _handler;
        private readonly JsonSerializerOptions _jsonSerializerOptions;
        private readonly KafkaConsumerConfiguration _configuration;

        protected KafkaConsumerBackgroundService(THandler handler, KafkaConsumerConfiguration configuration)
        {
            _handler = handler;
            _configuration = configuration;
            var consumerConfig = new ConsumerConfig
            {
                GroupId = _configuration.GroupId,
                BootstrapServers = _configuration.KafkaHost
            };
            _consumer = new ConsumerBuilder<string, string>(consumerConfig).Build();
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                Converters = {new JsonStringEnumConverter()},
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        protected sealed override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            new Thread(() => StartConsumerLoop(stoppingToken)).Start();
            return Task.CompletedTask;
        }

        private void StartConsumerLoop(CancellationToken cancellationToken)
        {
            _consumer.Subscribe(_configuration.TopicName);

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var cr = _consumer.Consume(cancellationToken);
                    var deserialize = JsonSerializer.Deserialize<TMessage>(cr.Message.Value, _jsonSerializerOptions);
                    _handler.Handle(deserialize);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (ConsumeException e)
                {
                    if (e.Error.IsFatal)
                    {
                        break;
                    }
                }
                catch (Exception e)
                {
                    break;
                }
            }
        }

        public override void Dispose()
        {
            _consumer.Close();
            _consumer.Dispose();
            base.Dispose();
        }
    }
}