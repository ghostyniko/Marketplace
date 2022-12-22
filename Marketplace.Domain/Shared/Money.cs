using Marketplace.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marketplace.Domain.Shared
{
    public class Money : Value<Money>
    {
        private const string DefaultCurrency = "EUR";
        public static Money FromDecimal(decimal amount, ICurrencyLookup currencyLookup, string currencyCode = DefaultCurrency) =>
            new Money(amount, currencyLookup, currencyCode);
        public static Money FromString(string amout, ICurrencyLookup currencyLookup, string currencyCode = DefaultCurrency) =>
            new Money(decimal.Parse(amout), currencyLookup, currencyCode);

        protected Money(decimal amount, ICurrencyLookup currencyLookup, string currencyCode = DefaultCurrency)
        {
            if (string.IsNullOrEmpty(currencyCode))
            {
                throw new ArgumentNullException(nameof(currencyCode), "Currency code must be specified");
            }
            var currency = currencyLookup.FindCurrency(currencyCode);

            if (!currency.InUse)
            {
                throw new ArgumentException($"Currency code {currencyCode} is not valid");
            }

            if (decimal.Round(amount, currency.DecimalPlaces) != amount)
            {
                throw new ArgumentOutOfRangeException(nameof(amount)
                    , "Ammount cannot have more than two values");
            }
            Amount = amount;
            Currency = currency;
        }

        protected Money(decimal amount, CurrencyDetails currency)
        {
            Amount = amount;
            Currency = currency;
        }

        protected Money() { }
        public decimal Amount { get; private set; }
        public CurrencyDetails Currency { get; private set; }

        public Money Add(Money summand)
        {
            if (Currency != summand.Currency)
            {
                throw new CurrencyMismatchException("Cannot sum ammounts with different currencies");
            }
            return new Money(Amount + summand.Amount, Currency);
        }

        public Money Subtract(Money substrahend)
        {
            if (Currency != substrahend.Currency)
            {
                throw new CurrencyMismatchException("Cannot substract ammounts with different currencies");
            }
            return new Money(Amount - substrahend.Amount, Currency);
        }


        public static Money operator +(Money summand1, Money summand2) =>
            summand1.Add(summand2);
        public static Money operator -(Money minuend, Money substrahend) =>
            minuend.Subtract(substrahend);

        public class CurrencyMismatchException : Exception
        {
            public CurrencyMismatchException(string message) :
            base(message)
            {
            }
        }


    }
}
