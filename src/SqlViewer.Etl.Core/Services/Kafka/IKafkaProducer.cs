using Confluent.Kafka;

namespace SqlViewer.Etl.Core.Services.Kafka;

public interface IKafkaProducer : IDisposable
{
    Task<DeliveryResult<string, string>> ProduceAsync(string topic, string key, string payload);
}
