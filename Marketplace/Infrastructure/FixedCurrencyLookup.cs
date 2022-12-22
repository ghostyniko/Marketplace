using Marketplace.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Infrastructure
{
    public class FixedCurrencyLookup : ICurrencyLookup
    {
        private static readonly IEnumerable<CurrencyDetails> _currencyList = new[]
         {
            new CurrencyDetails
            {
                CurrencyCode = "EUR",
                InUse = true,
                DecimalPlaces = 2
            },
            new CurrencyDetails
            {
                CurrencyCode = "USD",
                InUse = true,
                DecimalPlaces = 2
            },
            new CurrencyDetails
            {
                CurrencyCode = "JPY",
                InUse = true,
                DecimalPlaces = 0
            },
            new CurrencyDetails
            {
                CurrencyCode = "DEM",
                InUse = false,
                DecimalPlaces = 2
            },

        };

        public CurrencyDetails FindCurrency(string currencyCode)
        {
            var currency = _currencyList.FirstOrDefault(c => c.CurrencyCode == currencyCode);
            return currency ?? CurrencyDetails.None;
        }
    }
}
