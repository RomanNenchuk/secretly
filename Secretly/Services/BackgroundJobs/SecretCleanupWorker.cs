using Microsoft.EntityFrameworkCore;
using Secretly.Data;

namespace Secretly.Services.BackgroundJobs;

public class SecretCleanupWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public SecretCleanupWorker(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            
            var expirationTime = DateTime.UtcNow.AddHours(-24);
            
            // Масове видалення без завантаження в пам'ять (EF Core 7+)
            await context.SecretNotes
                .Where(n => n.CreatedAt < expirationTime)
                .ExecuteDeleteAsync(cancellationToken);

            await Task.Delay(TimeSpan.FromHours(1), cancellationToken);
        }
    }
}
