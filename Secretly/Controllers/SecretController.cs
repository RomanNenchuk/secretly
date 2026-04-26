// Controllers/SecretController.cs
using Microsoft.AspNetCore.Mvc;
using Secretly.Models.DTOs.Secrets;
using Secretly.Services;
using Secretly.Services.Interfaces;

namespace Secretly.Controllers;

public class SecretController : Controller
{
    private readonly ISecretService _secretService;

    public SecretController(ISecretService secretService)
    {
        _secretService = secretService;
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
        
        var isBase64 = System.Text.RegularExpressions.Regex.IsMatch(
            request.EncryptedContent, 
            @"^[a-zA-Z0-9\+/]*={0,2}$"
        );
        
        if (!isBase64)
        {
            return BadRequest(new { error = "Invalid data format. Nice try, hacker!" });
        }

        var id = await _secretService.CreateSecretAsync(request.EncryptedContent);
        return Json(new { id });
    }

    [HttpGet("/secret/{id:guid}")]
    public async Task<IActionResult> ViewSecret(Guid id)
    {
        var secretNote = await _secretService.GetAndDeleteSecretAsync(id);
        
        if (secretNote == null)
        {
            return View("NotFound");
        }
        
        return View(secretNote);
    }
}
