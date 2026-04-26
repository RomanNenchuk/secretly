using Microsoft.EntityFrameworkCore;
using Secretly.Data;
using Secretly.Models.Entities;
using Secretly.Services.Interfaces;

namespace Secretly.Services.Implementations;

public class SecretService : ISecretService
{
    private readonly ApplicationDbContext _context;

    public SecretService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> CreateSecretAsync(string encryptedContent)
    {
        var secret = new SecretNote
        {
            Id = Guid.NewGuid(),
            EncryptedContent = encryptedContent
        };

        _context.SecretNotes.Add(secret);
        await _context.SaveChangesAsync();
        return secret.Id;
    }

    public async Task<SecretNote?> GetAndDeleteSecretAsync(Guid id)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        
        var note = await _context.SecretNotes.FirstOrDefaultAsync(n => n.Id == id);
        if (note == null)
        {
            return null;
        }

        _context.SecretNotes.Remove(note);
        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        return note;
    }
}
