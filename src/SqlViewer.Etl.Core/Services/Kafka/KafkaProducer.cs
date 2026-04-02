using Confluent.Kafka;

namespace SqlViewer.Etl.Core.Services.Kafka;

public sealed class KafkaProducer : IKafkaProducer
{
    private readonly IProducer<string, string> _producer;

    public KafkaProducer(string bootstrapServers)
    {
        ProducerConfig config = new()
        {
            BootstrapServers = bootstrapServers,
            // Critical for ensuring no duplicates during network retries
            EnableIdempotence = true,
            Acks = Acks.All
        };
        _producer = new ProducerBuilder<string, string>(config).Build();
    }

    public async Task<DeliveryResult<string, string>> ProduceAsync(string topic, string key, string payload)
    {
        return await _producer.ProduceAsync(topic, new Message<string, string> { Key = key, Value = payload });
    }

    public void Dispose() => _producer.Dispose();
}
