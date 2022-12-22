using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Domain.Shared
{
    public static class DomainExceptions
    {
        public class InvalidEntityState : Exception
        {

            public InvalidEntityState(object entity, string message)
                : base($"Entity {entity.GetType().Name} state change rejected, { message}")
            {
            }
        }

        public class ProfanityFound : Exception
        {
            public ProfanityFound(string value)
                : base($"Profanity found in value {value}")
            {
            }

        }
    }
}
