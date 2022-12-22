using Marketplace.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Domain.UserProfile
{
    public class FullName:Value<FullName>
    {
        public static FullName FromString(string value)
        {
            CheckValidity(value);
            return new FullName(value);
        }
        
        private string _value;
        public string Value
        {
            get => _value;
            private set => _value = value;
        }

        internal FullName(string value)
        {
            _value = value;
        }
        private static void CheckValidity(string value)
        {
            if (value.IsEmpty())
            {
                throw new ArgumentNullException(nameof(value));
            }
        }

        // Satisfy the serialization requirements
        protected FullName()
        {
            Value = "";
        }
        public static implicit operator string(FullName self) => self._value;
    }
}
