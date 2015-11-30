using Logbook.Shared.Results;

namespace Logbook.Server.Contracts.Encryption
{
    public interface IJsonWebTokenService : IService
    {
        string Generate(string userId);
        Result<string> ValidateAndDecode(string jsonWebToken);
    }
}