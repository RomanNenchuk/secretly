using System.ComponentModel.DataAnnotations;

namespace Secretly.Models.Entities;

public class SecretNote
{
    [Key]
    public Guid Id { get; set; }

    // Рядок, зашифрований алгоритмом AES-GCM на стороні клієнта
    [Required]
    public string EncryptedContent { get; set; } = null!;

    // Час створення. Потрібен для нашого фонового Worker'а, який видалятиме старі записи
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}