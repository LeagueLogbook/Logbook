using System.Threading.Tasks;
using Logbook.Server.Contracts.Mapping;
using Logbook.Shared.Entities.Authentication;
using Logbook.Shared.Models.Authentication;

namespace Logbook.Server.Infrastructure.Mapping
{
    public class UserToUserModelMapper : IMapper<User, UserModel>
    {
        public Task<UserModel> MapAsync(User source)
        {
            var result = new UserModel
            {
                Id = source.Id,
                EmailAddress = source.EmailAddress,
                PreferredLanguage = source.PreferredLanguage
            };

            return Task.FromResult(result);
        }
    }
}