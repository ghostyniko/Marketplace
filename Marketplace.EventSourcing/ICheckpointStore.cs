using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.EventSourcing
{
    public interface ICheckpointStore
    {
        Task<long?> GetCheckpoint();
        Task StoreCheckpoint(long? checkpoint);
    }
}
