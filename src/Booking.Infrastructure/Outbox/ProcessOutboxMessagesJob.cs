using System.Data;
using Booking.Application.Abstractions.Clocks;
using Booking.Application.Abstractions.Data;
using Booking.Domain.Abstractions;
using Dapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Quartz;

namespace Booking.Infrastructure.Outbox
{
    internal record OutboxMessageResponse(Guid Id, string Content);

    [DisallowConcurrentExecution]
    public class ProcessOutboxMessagesJob : IJob
    {
        private static readonly JsonSerializerSettings JsonSerializerSettings = new()
        {
            TypeNameHandling = TypeNameHandling.All
        };
        private readonly ISqlConnectionFactory _sqlConnection;
        private readonly IPublisher _publisher;
        private readonly IDateTimeProvider _timeProvider;
        private readonly OutboxOptions _outboxOptions;
        private readonly ILogger<ProcessOutboxMessagesJob> _logger;

        public ProcessOutboxMessagesJob(ISqlConnectionFactory sqlConnection, IPublisher publisher, IDateTimeProvider timeProvider, IOptions<OutboxOptions> outboxOptions, ILogger<ProcessOutboxMessagesJob> logger)
        {
            _sqlConnection = sqlConnection;
            _publisher = publisher;
            _timeProvider = timeProvider;
            _outboxOptions = outboxOptions.Value;
            _logger = logger;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("Beginning to process outbox messages");

            using var connection = _sqlConnection.CreateConnection();
            using var transaction = connection.BeginTransaction();

            var outboxMessages = await GetOutboxMessagesAsync(connection, transaction);
            foreach (var outboxMessage in outboxMessages)
            {
                Exception? exception = null;
                try
                {
                    IDomainEvent domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(outboxMessage.Content, JsonSerializerSettings)!;
                    await _publisher.Publish(domainEvent, context.CancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Exception while processing outbox message {MessageId}", outboxMessage.Id);
                    exception = ex;
                }

                await UpdateOutboxMessageAsync(connection, transaction, outboxMessage, exception);
            }

            transaction.Commit();
            _logger.LogInformation("Completed processing outbox messages");
        }

        private async Task UpdateOutboxMessageAsync(IDbConnection connection, IDbTransaction transaction, OutboxMessageResponse outboxMessage, Exception? exception)
        {
            const string sql = @"
            UPDATE outbox_messages
            SET processed_on_utc = @ProcessedOnUtc,
                error = @Error
            WHERE id = @Id";

            await connection.ExecuteAsync(
            sql,
            new
            {
                outboxMessage.Id,
                ProcessedOnUtc = _timeProvider.UtcNow,
                Error = exception?.ToString()
            },
            transaction: transaction);
        }

        private async Task<IReadOnlyList<OutboxMessageResponse>> GetOutboxMessagesAsync(IDbConnection connection, IDbTransaction transaction)
        {
            string sql = $"""
                      SELECT id, content
                      FROM outbox_messages
                      WHERE processed_on_utc IS NULL
                      ORDER BY occurred_on_utc
                      LIMIT {_outboxOptions.BatchSize}
                      FOR UPDATE
                      """;

            IEnumerable<OutboxMessageResponse> outboxMessages = await connection.QueryAsync<OutboxMessageResponse>(
                sql,
                transaction: transaction);

            return outboxMessages.ToList();
        }
    }
}
