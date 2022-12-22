using Marketplace.Domain.Shared;
using Marketplace.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Domain.UserProfile
{
    public class UserProfile : AggregateRoot<UserId>
    {
        // Properties to handle the persistence
        private string DbId
        {
            get => $"UserProfile/{Id.Value}";
            set { }
        }

        public UserProfile(UserId id, FullName fullName, DisplayName displayName)
        {
            Apply(new Events.UserRegistered
            {
                DisplayName = displayName,
                UserId = id,
                FullName=fullName,
            }
            );
        }

        internal UserProfile() { }
        public FullName FullName { get; private set; }
        public DisplayName DisplayName { get; private set; }
        public string PhotoUrl { get; private set; }

        public void UpdateFullName(FullName newFullName)
        {
            Apply(new Events.UserFullNameUpdated
                {
                    FullName = newFullName,
                    UserId = Id,
                }
            );
        }

        public void UpdateDisplayName(DisplayName newDisplayName)
        {
            Apply(new Events.UserDisplayNameUpdated
            {
                DisplayName = newDisplayName,
                UserId = Id,
            }
            );
        }
        public void UpdatePhotoUrl(string newPhotoUrl)
        {
            Apply(new Events.ProfilePhotoUploaded
            {
                PhotoUrl = newPhotoUrl,
                UserId = Id,
            }
            );
        }

        protected override void EnsureValidState()
        {
            var valid =
                Id != null &&
                FullName != null &&
                DisplayName != null;
            if (!valid)
                throw new DomainExceptions.InvalidEntityState(
                this, $"Post-checks failed");
        }

        protected override void When(object @event)
        {
            switch (@event)
            {
                case Events.UserRegistered e:
                    Id = new UserId(e.UserId);
                    DisplayName = new DisplayName(e.DisplayName);
                    FullName = new FullName(e.FullName);
                    break;
                case Events.UserFullNameUpdated e:
                    FullName = new FullName(e.FullName);
                    break;
                case Events.UserDisplayNameUpdated e:
                    DisplayName = new DisplayName(e.DisplayName);
                    break;
                case Events.ProfilePhotoUploaded e:
                    PhotoUrl = e.PhotoUrl;
                    break;
            }
        }
    }
}
