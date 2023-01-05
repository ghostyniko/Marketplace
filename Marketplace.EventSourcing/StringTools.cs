using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.EventSourcing
{
    public static class StringTools
    {
        public static bool IsEmpty(this string value) => string.IsNullOrWhiteSpace(value);
    }
}
