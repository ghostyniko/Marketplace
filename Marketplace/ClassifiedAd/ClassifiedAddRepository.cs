using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Client.Documents.Session;
using Marketplace.Domain.ClassifiedAd;
using Marketplace.Infrastructure;

namespace Marketplace.ClassifiedAd
{
    public class ClassifiedAddRepository :RavenDbRepository<ClassifiedAdd,ClassifiedAddId>, IClassifiedAdRepository
    {
        public ClassifiedAddRepository(IAsyncDocumentSession session) : base(session, id=>EntityId(id))
        {
        }

        //private readonly IAsyncDocumentSession _session;
        //private readonly ClassifiedAdDbContext _context;
        //public ClassifiedAddRepository(ClassifiedAdDbContext context)
        //{
        //    _context = context;
        //}

        //public Task Add(ClassifiedAdd entity)
        //{
        //    return _context.ClassifiedAds.AddAsync(entity).AsTask();

        //}

        //public async Task<bool> Exists(ClassifiedAddId id)
        //{

        //    return await _context.ClassifiedAds.FindAsync(new Guid(id.Value)).AsTask() != null;
        //}

        //public Task<ClassifiedAdd> Load(ClassifiedAddId id)
        //{
        //    return _context.ClassifiedAds.FindAsync(new Guid(id.Value)).AsTask();
        //}

        private static string EntityId(ClassifiedAddId id)
            => $"ClassifiedAd/{id.Value.ToString()}";

    }
}
