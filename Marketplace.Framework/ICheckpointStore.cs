using EventStore.ClientAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Framework
{
    public interface ICheckpointStore
    {
        Task<Position> GetCheckpoint();
        Task StoreCheckpoint(Position position);
    }
}
