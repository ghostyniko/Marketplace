using Marketplace.Domain.ClassifiedAd;
using Marketplace.Domain.Shared;
using Marketplace.Framework;
using Serilog;
using static Marketplace.ClassifiedAd.Contracts;

namespace Marketplace.ClassifiedAd
{
    public class ClassifiedAdApplicationService : IApplicationService
    {
        private readonly IAggregateStore _store;
        private ICurrencyLookup _currencyLookup;
        public ClassifiedAdApplicationService(
                IAggregateStore store, ICurrencyLookup currencyLookup
        )
        {
            _currencyLookup = currencyLookup;
            _store = store;
        }
        public async Task Handle(object command)
        {
            switch (command)
            {
                case V1.Create cmd:
                    await HandleCreate(cmd);
                    break;
                case V1.SetTitle cmd:
                    await HandleUpdate(cmd.Id, ad => ad.SetTitle(ClassifiedAddTitle.FromString(cmd.Title)));
                    break;
                case V1.UpdateText cmd:
                    await HandleUpdate(cmd.Id, ad => ad.UpdateText(ClassifiedAddText.FromString(cmd.Text)));
                    break;
                case V1.UpdatePrice cmd:
                    await HandleUpdate(cmd.Id, ad => ad.UpdatePrice(Price.FromDecimal(cmd.Price, cmd.Currency, _currencyLookup)));
                    break;
                case V1.RequestToPublish cmd:
                    await HandleUpdate(cmd.Id, ad => ad.RequestToPublish());
                    break;
                case V1.Publish cmd:
                    await HandleUpdate(cmd.Id,ad=>ad.Publish(new UserId(cmd.Id)));
                    break;
                default:
                    throw new InvalidOperationException(
                        $"Command type {command.GetType().FullName} is unknown");
            }

        }

        private async Task HandleCreate(V1.Create cmd)
        {
            if (await _store.Exists<ClassifiedAdd, ClassifiedAddId>(new ClassifiedAddId(cmd.Id)))
                throw new InvalidOperationException(
                $"Entity with id {cmd.Id} already exists");

            var classifiedAd = new ClassifiedAdd(
                new ClassifiedAddId(cmd.Id),
                new UserId(cmd.OwnerId)
            );
            await _store.Save<ClassifiedAdd,ClassifiedAddId>(classifiedAd);
        }

        private async Task HandleUpdate(Guid id,Action<ClassifiedAdd> operation)
        {
            await this.HandleUpdate(_store,new ClassifiedAddId(id),operation);
        }

    }
}
