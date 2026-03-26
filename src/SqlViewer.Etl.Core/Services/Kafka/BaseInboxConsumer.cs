using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SqlViewer.Shared.Messages.Storage.Entities;

namespace SqlViewer.Etl.Core.Services.Kafka;

/// <summary>
/// Base class for Kafka consumers that implements the Inbox pattern.
/// Persists incoming messages to the database before processing.
/// </summary>
public abstract class BaseInboxConsumer<TKey, TValue> : BackgroundService
{
    private readonly ILogger _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly string _topic;
    private readonly ConsumerConfig _config;

    protected BaseInboxConsumer(
        ILogger logger,
        IServiceScopeFactory scopeFactory,
        string topic,
        string bootstrapServers,
        string groupId)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
        _topic = topic;
        _config = new ConsumerConfig
        {
            BootstrapServers = bootstrapServers,
            GroupId = groupId,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = false
        };
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using IConsumer<TKey, TValue> consumer = new ConsumerBuilder<TKey, TValue>(_config)
            .SetErrorHandler((_, e) => _logger.LogError("Kafka error: {Reason}", e.Reason))
            .Build();
        consumer.Subscribe(_topic);

        _logger.LogInformation("Started Kafka consumer for topic: {Topic}", _topic);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                ConsumeResult<TKey, TValue> result = consumer.Consume(stoppingToken);
                if (result == null) continue;

                _logger.LogDebug("Received message from Kafka. Key: {Key}", result.Message.Key);

                await SaveToInboxAsync(result, stoppingToken);

                // Commit offset only after the message is safely in our Database
                consumer.Commit(result);
            }
            catch (OperationCanceledException) { break; }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while consuming Kafka message from topic {Topic}", _topic);
                await Task.Delay(1000, stoppingToken); // Prevent log flooding on persistent errors
            }
        }
    }

    private async Task SaveToInboxAsync(ConsumeResult<TKey, TValue> result, CancellationToken ct)
    {
        using IServiceScope scope = _scopeFactory.CreateScope();

        InboxMessageEntity inboxMessage = CreateInboxEntity(result.Message.Value);

        // Here we need a way to get the generic DB context or a specific Inbox service.
        IInboxService inboxService = scope.ServiceProvider.GetRequiredService<IInboxService>();
        await inboxService.StoreMessageAsync(inboxMessage, ct);
    }

    /// <summary>
    /// Logic to extract CorrelationId from the message payload or headers.
    /// </summary>
    protected abstract InboxMessageEntity CreateInboxEntity(TValue value);
}
