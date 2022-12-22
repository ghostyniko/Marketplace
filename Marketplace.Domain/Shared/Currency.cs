using Marketplace.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Domain.Shared
{
    public interface ICurrencyLookup
    {
        CurrencyDetails FindCurrency(string currencyCode);
    }
    public class CurrencyDetails : Value<CurrencyDetails>
    {
        public string CurrencyCode { get; set; }
        public bool InUse { get; set; }
        public int DecimalPlaces { get; set; }
        // Satisfy the serialization requirements
        public CurrencyDetails() { }
        public static CurrencyDetails None = new CurrencyDetails
        {
            InUse = false,
            DecimalPlaces = 0,
            CurrencyCode = ""
        };
    }
}
