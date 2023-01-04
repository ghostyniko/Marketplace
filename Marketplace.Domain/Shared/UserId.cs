using Marketplace.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Domain.Shared
{
    public class UserId : Value<UserId>
    {
        public static UserId NoUser =
            new UserId();

        public Guid Value { get; private set; }
        
        public UserId(Guid value)
        {
            if (value == default)
            {
                throw new ArgumentNullException(nameof(value), "User id cannot be empty");
            }
            Value = value;
        }
        internal UserId() { }
        public static implicit operator Guid(UserId self) => self.Value;
        public override string ToString() => Value.ToString();
        
    }
}
