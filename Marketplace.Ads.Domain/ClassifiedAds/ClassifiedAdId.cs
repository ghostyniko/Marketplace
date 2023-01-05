using Marketplace.EventSourcing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Ads.Domain.ClassifiedAds
{
    public class ClassifiedAdId : AggregateId<ClassifiedAd>
    {
        ClassifiedAdId(Guid value) : base(value) { }

        public static ClassifiedAdId FromGuid(Guid value)
            => new ClassifiedAdId(value);
    }
}
