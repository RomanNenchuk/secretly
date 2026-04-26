using Secretly.Models.Entities;

namespace Secretly.Services.Interfaces;

public interface ISecretService
{
    Task<Guid> CreateSecretAsync(string encryptedContent);
    Task<SecretNote?> GetAndDeleteSecretAsync(Guid id);
}
