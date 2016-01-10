namespace Logbook.Server.Contracts.Encryption
{
    public interface IHashingService : IService
    {
        byte[] ComputeSHA256Hash(byte[] data);
    }
}