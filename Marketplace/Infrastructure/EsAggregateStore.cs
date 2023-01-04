using EventStore.ClientAPI;
using Marketplace.Framework;
using Newtonsoft.Json;
using System.Text;

namespace Marketplace.Infrastructure
{
    public class EsAggregateStore:IAggregateStore
    {
        private readonly IEventStoreConnection _connection;
        public EsAggregateStore(IEventStoreConnection connection)
        {
            _connection = connection;
        }

        private static string GetStreamName<T, TId>(TId aggregateId) where T : AggregateRoot<TId> where TId : Value<TId>
        {
            return $"{typeof(T).Name}-{aggregateId.ToString()}";
        }
        private static string GetStreamName<T, TId>(T aggregate) where T : AggregateRoot<TId> where TId : Value<TId>
        {
            return $"{typeof(T).Name}-{aggregate.Id.ToString()}";

        }
        private static byte[] Serialize(object data)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
        }

        public async Task<bool> Exists<T, TId>(TId aggregateId)
            where T : AggregateRoot<TId>
            where TId : Value<TId>
        {
            var stream = GetStreamName<T, TId>(aggregateId);
          
            var result = await _connection.ReadEventAsync(stream, 1, false);
            return result.Status != EventReadStatus.NoStream;
        }

        public async Task<T> Load<T, TId>(TId aggregateId)
            where T : AggregateRoot<TId>
            where TId : Value<TId>
        {
            if (aggregateId == null)
                throw new ArgumentNullException(nameof(aggregateId));
            
            var stream = GetStreamName<T, TId>(aggregateId);
            var aggregate = (T) Activator.CreateInstance(typeof(T),true);

            var page = await _connection.ReadStreamEventsForwardAsync(
                stream,0,1024,false);
            aggregate.Load(page.Events.Select(
                resolvedEvent=>resolvedEvent.Deserialize())
                .ToArray());
            return aggregate;

        }

        public async Task Save<T, TId>(T aggregate)
            where T : AggregateRoot<TId>
            where TId : Value<TId>
        {
            if (aggregate == null)
                throw new ArgumentNullException(nameof(aggregate));

            var changes = aggregate.GetChanges()
                .Select(@event => new EventData(
                Guid.NewGuid(), 
                @event.GetType().Name, 
                true, Serialize(@event), 
                Serialize(new EventMetadata { ClrType=@event.GetType().AssemblyQualifiedName})
                ))
                .ToArray();
            if (!changes.Any()) return;

            var streamName = GetStreamName<T,TId>(aggregate);
            await _connection.AppendToStreamAsync(streamName, aggregate.Version, changes);
            aggregate.ClearChanges();
          
        }

        
    }
    internal class EventMetadata
    {
        public string ClrType { get; set; }
    }

}
