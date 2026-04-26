using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Secretly.Models;
using Secretly.Data;
using Secretly.Models.DTOs.Secrets;
using Secretly.Models.Entities;

namespace Secretly.Controllers;

public class SecretController : Controller
{
    private readonly ApplicationDbContext _context;

    public SecretController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("/")]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost("/secret")]
    public async Task<IActionResult> CreateSecret([FromBody] CreateSecretRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.EncryptedContent))
        {
            return BadRequest(new { error = "Content cannot be empty." });
        }

        var secretNote = new SecretNote
        {
            Id = Guid.NewGuid(),
            EncryptedContent = request.EncryptedContent,
            CreatedAt = DateTime.UtcNow
        };

        _context.SecretNotes.Add(secretNote);
        await _context.SaveChangesAsync();

        return Json(new { id = secretNote.Id });
    }

    [HttpGet("/secret/{id:guid}")]
    public async Task<IActionResult> ViewSecret(Guid id)
    {
        // Тут ми пізніше додамо логіку "атомарного читання та видалення"
        return View(); // Поверне Views/Secret/ViewSecret.cshtml
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
