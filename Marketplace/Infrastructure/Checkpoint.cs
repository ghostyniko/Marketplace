using EventStore.ClientAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Infrastructure
{
    public class Checkpoint
    {
        public string Id { get; set; }
        public Position Position { get; set; }
    }
}
