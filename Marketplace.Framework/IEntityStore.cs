using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Framework
{
    public interface IEntityStore
    {
        /// <summary>
        /// Loads an entity by id
        /// </summary>
        Task<T> Load<T>(string entityId);
        /// <summary>
        /// Persists an entity
        /// </summary>
        Task Save<T>(T entity);
        /// <summary>
        /// Check if entity with a given id already exists
        /// <typeparam name="T">Entity type</typeparam>
        Task<bool> Exists<T>(string entityId);
    }
}
