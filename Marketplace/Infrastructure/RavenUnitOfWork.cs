using Marketplace.Framework;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Infrastructure
{
    public class RavenUnitOfWork : IUnitOfWork
    {
        private readonly IAsyncDocumentSession _session;
        public RavenUnitOfWork(IAsyncDocumentSession session)
        {
            _session = session;
        }
        public Task Commit()
        {
            return _session.SaveChangesAsync();
        }
    }
}
