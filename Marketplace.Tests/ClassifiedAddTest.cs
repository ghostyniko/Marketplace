using Marketplace.Domain.ClassifiedAd;
using Marketplace.Domain.Shared;
using System;
using Xunit;

namespace Marketplace.Tests
{
    public class ClassifiedAdd_Publish_Spec
    {
        private readonly ClassifiedAdd _classifiedAdd;
        private readonly ICurrencyLookup _currencyLookup;
        public ClassifiedAdd_Publish_Spec()
        {
            _classifiedAdd = new ClassifiedAdd(
            new ClassifiedAddId(Guid.NewGuid()),
            new UserId(Guid.NewGuid()));
            _currencyLookup = new FakeCurrencyLookup();
        }

        [Fact]
        public void Price_update_is_correct()
        {
            _classifiedAdd.UpdatePrice(Price.FromDecimal(5, "EUR", _currencyLookup));
            Assert.Equal(2, _classifiedAdd.Price.Currency.DecimalPlaces);
        }
        [Fact]
        public void Can_Publish_a_valid_add()
        {
            _classifiedAdd.UpdatePrice(Price.FromDecimal(5, "EUR", _currencyLookup));
            _classifiedAdd.UpdateText(ClassifiedAddText.FromString("Add description"));
            _classifiedAdd.SetTitle(ClassifiedAddTitle.FromString("Add title"));
            _classifiedAdd.RequestToPublish();
            Assert.Equal(ClassifiedAdd.ClassifiedAdState.PendingReview, _classifiedAdd.State);
        }

        [Fact]
        public void Cannot_publish_without_title()
        {
            _classifiedAdd.UpdatePrice(Price.FromDecimal(5,"EUR",_currencyLookup));
            _classifiedAdd.UpdateText(ClassifiedAddText.FromString("Add description"));
            Assert.Throws<DomainExceptions.InvalidEntityState>(() => _classifiedAdd.RequestToPublish());
        }
        [Fact]
        public void Cannot_publish_without_text()
        {
            _classifiedAdd.SetTitle(ClassifiedAddTitle.FromString("Add title"));
            _classifiedAdd.UpdatePrice(Price.FromDecimal(5, "EUR", _currencyLookup));

            Assert.Throws<DomainExceptions.InvalidEntityState>(() => _classifiedAdd.RequestToPublish());
        }
        [Fact]
        public void Cannot_publish_without_price()
        {
            _classifiedAdd.UpdateText(ClassifiedAddText.FromString("Add description"));
            _classifiedAdd.SetTitle(ClassifiedAddTitle.FromString("Add title"));
            Assert.Throws<DomainExceptions.InvalidEntityState>(() => _classifiedAdd.RequestToPublish());
        }

        [Fact]
        public void Cannot_publish_with_zero_price()
        {
            _classifiedAdd.UpdatePrice(Price.FromDecimal(0.0m, "EUR", _currencyLookup));
            _classifiedAdd.UpdateText(ClassifiedAddText.FromString("Add description"));
            _classifiedAdd.SetTitle(ClassifiedAddTitle.FromString("Add title"));
            Assert.Throws<DomainExceptions.InvalidEntityState>(() => _classifiedAdd.RequestToPublish());
        }
    }
}
