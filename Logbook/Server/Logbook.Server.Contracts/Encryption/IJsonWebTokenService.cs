using Logbook.Shared;
using Logbook.Shared.Models;

namespace Logbook.Server.Contracts.Encryption
{
    public interface IJsonWebTokenService : IService
    {
        AuthenticationToken Generate(string userId);
        string ValidateAndDecode(string jsonWebToken);
    }
}