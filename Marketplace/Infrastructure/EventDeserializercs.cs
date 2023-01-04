using EventStore.ClientAPI;
using Newtonsoft.Json;
using System.Text;

namespace Marketplace.Infrastructure
{
    public static class EventDeserializercs
    {
        public static object Deserialize(this ResolvedEvent resolvedEvent)
        {

            var meta = JsonConvert.DeserializeObject<EventMetadata>(
                    Encoding.UTF8.GetString(resolvedEvent.Event.Metadata));
            var dataType = Type.GetType(meta.ClrType);
            var jsonData = Encoding.UTF8.GetString(resolvedEvent.Event.Data);
            var data = JsonConvert.DeserializeObject(jsonData, dataType);
            return data;
        }
    }
}
