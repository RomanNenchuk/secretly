using Microsoft.EntityFrameworkCore;
using Secretly.Models.Entities;

namespace Secretly.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : base(options)
    {
    }

    // Представляє нашу таблицю в базі даних
    public DbSet<SecretNote> SecretNotes { get; set; }

    // Налаштування моделі за допомогою Fluent API
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Створюємо індекс для колонки CreatedAt.
        // Це значно прискорить роботу Worker'а при пошуку нотаток для видалення.
        modelBuilder.Entity<SecretNote>()
            .HasIndex(n => n.CreatedAt);
    }
}
