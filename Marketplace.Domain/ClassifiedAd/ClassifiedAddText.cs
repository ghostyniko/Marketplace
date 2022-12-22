using Marketplace.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Domain.ClassifiedAd
{
    public class ClassifiedAddText : Value<ClassifiedAddText>
    {
        public static ClassifiedAddText NoText =
            new ClassifiedAddText();
        public static ClassifiedAddText FromString(string value) =>
           new ClassifiedAddText(value);
        private string _value;
        public string Value
        {
            get => _value;
            private set => _value = value;
        }
        // Satisfy the serialization requirements
        protected ClassifiedAddText()
        {
            Value = "";
        }
        internal ClassifiedAddText(string value)
        {
            _value = value;
        }

        public static implicit operator string(ClassifiedAddText self) => self._value;

    }
}
