using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Framework
{
    public abstract class Value<T> : IEquatable<T>
    {
        public bool Equals(T? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return CheckPropertiesEquality(other);
        }
        private bool CheckPropertiesEquality(T? other)
        {
            var properties = typeof(T).GetProperties();
            foreach (var properyInfo in properties)
            {
                var thisValue = properyInfo.GetValue(this);
                var otherValue = properyInfo.GetValue(other);
                if (!thisValue.Equals(otherValue)) return false;
            }
            return true;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((T)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                var properties = typeof(T).GetProperties();
                foreach (var properyInfo in properties)
                {
                    var propertyValue = properyInfo.GetValue(this);
                    hash = hash * 23 + propertyValue.GetHashCode();
                }
                return hash;

            }
        }

        public static bool operator == (Value<T> left, Value<T> right) =>
       Equals(left, right);
        public static bool operator != (Value<T> left, Value<T> right) =>
        !Equals(left, right);
    }
}
