using Logbook.Shared;

namespace Logbook.Server.Contracts.Encryption
{
    public interface IJsonWebTokenService : IService
    {
        string Generate(string userId);
        string ValidateAndDecode(string jsonWebToken);
    }
}