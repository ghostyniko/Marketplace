using EventStore.ClientAPI;
using Marketplace.ClassifiedAd;
using Marketplace.Domain.ClassifiedAd;
using Marketplace.Framework;
using Serilog.Events;
using System.Text.Json;
using ILogger = Serilog.ILogger;

namespace Marketplace.Infrastructure
{
    public class ProjectionsManager
    {
        private static readonly ILogger Log =
        Serilog.Log.ForContext<ProjectionsManager>();

        private readonly IEventStoreConnection _connection;
        private readonly IProjection[] _projections;
        private EventStoreAllCatchUpSubscription _subscription;
        private ICheckpointStore _checkpointStore;
        public ProjectionsManager(IEventStoreConnection connection,ICheckpointStore checkpointStore,
        params IProjection[] projections)
        {
            _connection = connection;
            _checkpointStore = checkpointStore;
            _projections = projections;
        }

        public async Task Start()
        {
            var settings = new CatchUpSubscriptionSettings(2000, 500,
            Log.IsEnabled(LogEventLevel.Verbose),
            false, "try-out-subscription");
            var position = await _checkpointStore.GetCheckpoint();
            Log.Information("Getting position {position}",position);

            _subscription = _connection.SubscribeToAllFrom(
            position, settings, EventAppeared);
        }
        private async Task EventAppeared(
        EventStoreCatchUpSubscription
        subscription, ResolvedEvent resolvedEvent)
        {
            //Log.Information(JsonSerializer.Serialize(resolvedEvent));
            if (resolvedEvent.Event.EventType.StartsWith("$"))
                return;
            var @event = resolvedEvent.Deserialize();
            Log.Information("Projecting event {type}",
            @event.GetType().Name);
            await Task.WhenAll(_projections.Select
                (projection => projection.Project(@event)));
            await _checkpointStore.StoreCheckpoint(
                resolvedEvent.OriginalPosition.Value);
        }
        

        public void Stop() => _subscription.Stop();
    }
}
