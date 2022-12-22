using Marketplace.Domain.Shared;
using Marketplace.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Domain.UserProfile
{
    public class DisplayName:Value<DisplayName>
    {
        public static DisplayName FromString(string value, CheckTextForProfanity hasProfanity)
        {
            CheckValidity(value);
            if (hasProfanity(value))
                throw new DomainExceptions.ProfanityFound(value);
            return new DisplayName(value);
        }

        private string _value;
        public string Value
        {
            get => _value;
            private set => _value = value;
        }

        internal DisplayName(string value)
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
        protected DisplayName()
        {
            Value = "";
        }
        public static implicit operator string(DisplayName self) => self._value;
    }
}
