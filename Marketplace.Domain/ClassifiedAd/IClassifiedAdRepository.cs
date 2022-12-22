using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Domain.ClassifiedAd
{
    public interface IClassifiedAdRepository
    {
        Task<ClassifiedAdd> Load(ClassifiedAddId id);

        Task Add(ClassifiedAdd entity);
        Task<bool> Exists(ClassifiedAddId id);
    }
}
