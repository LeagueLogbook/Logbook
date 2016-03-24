namespace Logbook.Server.Contracts.Encryption
{
    public interface IEncryptionService : IService
    {
        byte[] Encrypt<T>(T payload, string password);
        T Decrypt<T>(byte[] data, string password);
    }
}