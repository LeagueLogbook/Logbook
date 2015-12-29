using System;
using Logbook.Shared;
using Logbook.Shared.Models;

namespace Logbook.Server.Contracts.Encryption
{
    public interface IJsonWebTokenService : IService
    {
        JsonWebToken Generate<T>(T payload, TimeSpan validDuration, string password);
        T ValidateAndDecode<T>(string jsonWebToken, string password);
    }
}