using Marketplace.Domain.Shared;
using System;
using Xunit;
using static Marketplace.Domain.Shared.Money;

namespace Marketplace.Tests
{
    public class MoneyTest
    {
        private static readonly ICurrencyLookup currencyLookup =
               new FakeCurrencyLookup();
        [Fact]
        public void
        Money_objects_with_the_same_amount_should_be_equal()
        {
            var firstAmount = Money.FromDecimal(5, currencyLookup, "EUR");
            var secondAmount = Money.FromDecimal(5, currencyLookup, "EUR");
            Assert.Equal(firstAmount, secondAmount);
        }

        [Fact]
        public void Two_of_same_amount_but_differentCurrencies_should_not_be_equal()
        {
            var firstAmount = Money.FromDecimal(5, currencyLookup, "EUR");
            var secondAmount = Money.FromDecimal(5,currencyLookup, "USD");
            Assert.NotEqual(firstAmount, secondAmount);
        }

        [Fact]
        public void From_string_and_from_decimal_should_be_equal()
        {
            var firstAmount = Money.FromDecimal(5, currencyLookup, "EUR");
            var secondAmount = Money.FromString("5,00", currencyLookup, "EUR");
            Assert.Equal(firstAmount, secondAmount);
        }

        [Fact]
        public void Unused_currency_should_not_be_allowed()
        {
            Assert.Throws<ArgumentException>(() => Money.FromDecimal(5, currencyLookup, "DEM"));
        }

        [Fact]
        public void Nonexistent_currency_should_not_be_allowed()
        {
            Assert.Throws<ArgumentException>(() => Money.FromDecimal(5, currencyLookup, "What"));
        }

        [Fact]
        public void Too_many_decimal_places_should_not_be_allowed()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Money.FromDecimal(5.443m, currencyLookup, "EUR"));
        }
        [Fact]
        public void Sum_of_two_diffenrent_currency_ammounts_should_throw()
        {
            var amount1 = Money.FromDecimal(1, currencyLookup, "EUR");
            var amount2 = Money.FromDecimal(2, currencyLookup, "USD");
            Assert.Throws<CurrencyMismatchException>(() => amount1 + amount2);
        }

        [Fact]
        public void Subtract_of_two_diffenrent_currency_ammounts_should_throw()
        {
            var amount1 = Money.FromDecimal(1, currencyLookup, "EUR");
            var amount2 = Money.FromDecimal(2, currencyLookup, "USD");
            Assert.Throws<CurrencyMismatchException>(() => amount1 - amount2);
        }

        [Fact]
        public void Sum_of_money_gives_full_amount()
        {
            var coin1 = Money.FromDecimal(1, currencyLookup, "USD");
            var coin2 = Money.FromDecimal(2, currencyLookup, "USD");
            var coin3 = Money.FromDecimal(2, currencyLookup, "USD");
            var banknote = Money.FromDecimal(5, currencyLookup, "USD");
            Assert.Equal(banknote, coin1 + coin2 + coin3);
        }
    }
}
