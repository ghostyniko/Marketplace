using Marketplace.Domain.Shared;
using Marketplace.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Marketplace.Domain.ClassifiedAd
{
    public class ClassifiedAdd : AggregateRoot<ClassifiedAddId>
    {
        // Properties to handle the persistence
        private string DbId
        {
            get => $"ClassifiedAd/{Id}";
            set { }
        }

        //public Guid ClassifiedAddId { get; private set; }
        //public ClassifiedAddId Id { get; set; }

        public ClassifiedAdd(ClassifiedAddId id, UserId ownerId)
        {

            Apply(new Events.ClassifiedAddCreated
            {
                Id = id,
                OwnerId = ownerId

            }
            );
        }

        internal ClassifiedAdd() { }
        public UserId OwnerId { get; private set; }
        public ClassifiedAddTitle Title { get; private set; }
        public ClassifiedAddText Text { get; private set; }
        public Price Price { get; private set; }
        public ClassifiedAdState State { get; private set; }
        public IList<Picture> Pictures { get; private set; }
        public UserId ApprovedBy { get; private set; }

        public void SetTitle(ClassifiedAddTitle title)
        {

            Apply(new Events.ClassifiedAddTitleChanged
            {
                Id = Id,
                Title = title

            });
        }

        public void UpdateText(ClassifiedAddText text)
        {

            Apply(new Events.ClassifiedAddTextChanged
            {
                Id = Id,
                Text = text,

            });
        }
        public void UpdatePrice(Price price)
        {
            Apply(new Events.ClassifiedAddPriceChanged
            {
                Id = Id,
                Price = price.Amount,
                CurrencyCode = price.Currency.CurrencyCode,

            });

        }

        public void RequestToPublish()
        {

            Apply(new Events.ClassifiedAddSentForReview
            {
                Id = Id,
            });
        }
        public void Publish(UserId approvedBy)
        {
            Apply(new Events.ClassifiedAddApproved
            {
                Id = Id,
                ApprovedBy = approvedBy,
            });
        }

        public void AddPicture(Uri pictureUri, PictureSize size) =>
        Apply(new Events.PictureAddedToAClassifiedAd
        {
            PictureId = new Guid(),
            ClassifiedAdId = Id,
            Url = pictureUri.ToString(),
            Height = size.Height,
            Width = size.Width,
            Order = Pictures.Max(x => x.Order)
        });

        public void ResizePicture(PictureId pictureId, PictureSize newSize)
        {
            var picture = FindPicture(pictureId);
            if (picture == null)
                throw new InvalidOperationException(
                "Cannot resize a picture that I don't have");
            picture.Resize(newSize);
        }


        private Picture FindPicture(PictureId id)
        => Pictures.FirstOrDefault(x => x.Id == id);
        private Picture FirstPicture
             => Pictures.OrderBy(x => x.Order).FirstOrDefault();

        protected override void EnsureValidState()
        {

            var valid =
                Id != null &&
                OwnerId != null &&
                State switch
                {
                    ClassifiedAdState.PendingReview =>
                        Title != null
                         && Text != null
                         && Price?.Amount > 0
                         && (FirstPicture == null || FirstPicture.HasCorrectSize()),
                    ClassifiedAdState.Active =>
                        Title != null
                         && Text != null
                         && Price?.Amount > 0
                         && (FirstPicture == null || FirstPicture.HasCorrectSize())
                         && ApprovedBy != null,
                    _ => true

                };

            if (!valid)
                throw new DomainExceptions.InvalidEntityState(
                this, $"Post-checks failed in state {State}");
        }

        protected override void When(object @event)
        {
            switch (@event)
            {
                case Events.ClassifiedAddCreated e:
                    Id = new ClassifiedAddId(e.Id);
                    OwnerId = new UserId(e.OwnerId);
                    State = ClassifiedAdState.Inactive;
                    //ClassifiedAddId = e.Id;
                    Pictures = new List<Picture>();
                    Title = ClassifiedAddTitle.NoTitle;
                    Text = ClassifiedAddText.NoText;
                    Price = Price.NoPrice;
                    ApprovedBy = UserId.NoUser;
                    break;
                case Events.ClassifiedAddTitleChanged e:
                    Title = ClassifiedAddTitle.FromString(e.Title);
                    break;
                case Events.ClassifiedAddTextChanged e:
                    Text = ClassifiedAddText.FromString(e.Text);
                    break;
                case Events.ClassifiedAddPriceChanged e:
                    Console.WriteLine(JsonSerializer.Serialize(e));
                    Price = new Price(e.Price, e.CurrencyCode);
                    break;
                case Events.ClassifiedAddSentForReview e:
                    State = ClassifiedAdState.PendingReview;
                    break;
                case Events.ClassifiedAddApproved e:
                    ApprovedBy = new UserId(e.ApprovedBy);
                    State = ClassifiedAdState.Active;
                    break;
                case Events.PictureAddedToAClassifiedAd e:
                    var picture = new Picture(Apply);
                    ApplyToEntity(picture, e);
                    Pictures.Add(picture);
                    break;

            }
        }

        public enum ClassifiedAdState
        {
            PendingReview,
            Active,
            Inactive,
            MarkedAsSold
        }


    }
}
