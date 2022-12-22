using Marketplace.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Domain.ClassifiedAd
{
    public class ClassifiedAddTitle : Value<ClassifiedAddTitle>
    {
        public static ClassifiedAddTitle NoTitle =
            new ClassifiedAddTitle();

        public static ClassifiedAddTitle FromString(string value)
        {
            CheckValidity(value);
            return new ClassifiedAddTitle(value);
        }

        public string Value { get; private set; }

        // Satisfy the serialization requirements
        protected ClassifiedAddTitle()
        {
            Value = "";
        }
        internal ClassifiedAddTitle(string value)
        {
            Value = value;
        }
        private static void CheckValidity(string value)
        {
            if (value.Length > 100)
            {
                throw new ArgumentException("Title length cannot exceed 100 characters",
                    nameof(value));
            }
        }
        public static implicit operator string(ClassifiedAddTitle self) => self.Value;
    }
}
