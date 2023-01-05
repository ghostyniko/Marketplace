using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.EventSourcing
{
    public interface ISubscription
    {
        Task Project(object @event);
    }
}
