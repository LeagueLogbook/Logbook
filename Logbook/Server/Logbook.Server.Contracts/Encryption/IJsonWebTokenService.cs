using Logbook.Shared;
using Logbook.Shared.Models;

namespace Logbook.Server.Contracts.Encryption
{
    public interface IJsonWebTokenService : IService
    {
        JsonWebToken Generate(string userId);
        string ValidateAndDecode(string jsonWebToken);
    }
}