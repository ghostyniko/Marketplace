using Marketplace.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Domain.ClassifiedAd
{
    public class Price : Money
    {
        public static Price NoPrice =
            new Price(0, "");

        private Price(
        decimal amount,
        string currencyCode,
        ICurrencyLookup currencyLookup
        ) : base(amount, currencyLookup, currencyCode)
        {
            if (amount < 0)
                throw new ArgumentException(
                "Price cannot be negative",
                nameof(amount));
        }
        internal Price(decimal amount, string currencyCode)
        : base(amount, new CurrencyDetails
        {
            CurrencyCode = currencyCode
        })
        { }

        protected Price():base() { }

        public static Price FromDecimal(decimal amount, string
        currency,
        ICurrencyLookup currencyLookup) =>
        new Price(amount, currency, currencyLookup);
    }
}
