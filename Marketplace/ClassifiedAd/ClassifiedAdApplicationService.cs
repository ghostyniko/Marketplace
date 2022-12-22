using Marketplace.Domain.ClassifiedAd;
using Marketplace.Domain.Shared;
using Marketplace.Framework;
using Serilog;
using static Marketplace.ClassifiedAd.Contracts;

namespace Marketplace.ClassifiedAd
{
    public class ClassifiedAdApplicationService : IApplicationService
    {
        private readonly IClassifiedAdRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private ICurrencyLookup _currencyLookup;
        public ClassifiedAdApplicationService(
        IClassifiedAdRepository repository, ICurrencyLookup currencyLookup, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _currencyLookup = currencyLookup;
            _unitOfWork = unitOfWork;
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
            if (await _repository.Exists(new ClassifiedAddId(cmd.Id)))
            {
                throw new InvalidOperationException($"Entity with {cmd.Id} already exists");
            }

            var classifiedAd = new ClassifiedAdd(new ClassifiedAddId(cmd.Id), new UserId(cmd.OwnerId));
            Log.Information($"{classifiedAd.Id}");
            await _repository.Add(classifiedAd);
            await _unitOfWork.Commit();

        }

        private async Task HandleUpdate(Guid id, Action<ClassifiedAdd> operation)
        {
            var classifiedAd = await _repository.Load(new ClassifiedAddId(id));
            if (classifiedAd == null)
            {
                throw new InvalidOperationException($"Entity with {id} does not exists");
            }
            operation(classifiedAd);
            await _unitOfWork.Commit();

        }

    }
}
