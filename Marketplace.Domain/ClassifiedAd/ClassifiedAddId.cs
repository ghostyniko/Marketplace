using Marketplace.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Domain.ClassifiedAd
{
    public class ClassifiedAddId : Value<ClassifiedAddId>
    {
        public Guid Value { get; private set; }
       
        public ClassifiedAddId(Guid value)
        {
            if (value == default)
            {
                throw new ArgumentNullException(nameof(value), "Classified Add id cannot be empty");
            }
            Value = value;

        }
        internal ClassifiedAddId() { }
        public static implicit operator Guid(ClassifiedAddId self) => self.Value;
        public static implicit operator ClassifiedAddId(string value) => 
            new ClassifiedAddId(Guid.Parse(value));

        public override string ToString() => Value.ToString();

    }
}
