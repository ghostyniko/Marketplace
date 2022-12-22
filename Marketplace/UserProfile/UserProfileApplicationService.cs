using Marketplace.Domain.Shared;
using Marketplace.Domain.UserProfile;
using Marketplace.Framework;
using static Marketplace.UserProfile.Contracts;

namespace Marketplace.UserProfile
{
    public class UserProfileApplicationService : IApplicationService
    {
        private readonly IUserProfileRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly CheckTextForProfanity _checkText;

        public UserProfileApplicationService(IUserProfileRepository repository, IUnitOfWork unitOfWork, CheckTextForProfanity checkText)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _checkText = checkText;
        }

        public async Task Handle(object command)
        {
            switch (command)
            {
                case V1.RegisterUser cmd:
                    await HandleCreate(cmd);
                    break;
                case V1.UpdateUserDisplayName cmd:
                    await HandleUpdate(cmd.UserId,
                        userProfile => userProfile.UpdateDisplayName(DisplayName.FromString(cmd.DisplayName, _checkText)));
                    break;
                case V1.UpdateUserProfilePhoto cmd:
                    await HandleUpdate(cmd.UserId,
                       userProfile => userProfile.UpdatePhotoUrl(cmd.PhotoUrl));
                    break;
                case V1.UpdateUserFullName cmd:
                    await HandleUpdate(cmd.UserId,
                      userProfile => userProfile.UpdateFullName(FullName.FromString(cmd.FullName)));
                    break;
                default:
                    throw new InvalidOperationException(
                        $"Command type {command.GetType().FullName} is unknown");
            }
        }

        private async Task HandleCreate(V1.RegisterUser cmd)
        {
            if (await _repository.Exists(new UserId(cmd.UserId)))
            {
                throw new InvalidOperationException($"Entity with {cmd.UserId} already exists");
            }

            var userProfile = new Marketplace.Domain.UserProfile.UserProfile(new UserId(cmd.UserId),FullName.FromString(cmd.FullName),DisplayName.FromString(cmd.DisplayName,_checkText ));

            await _repository.Add(userProfile);
            await _unitOfWork.Commit();
        }

        private async Task HandleUpdate(Guid id, Action<Marketplace.Domain.UserProfile.UserProfile> operation)
        {
            var userProfile = await _repository.Load(new UserId(id));
            if (userProfile == null)
            {
                throw new InvalidOperationException($"Entity with {id} does not exists");
            }
            operation(userProfile);
            await _unitOfWork.Commit();
        }
    }
}
