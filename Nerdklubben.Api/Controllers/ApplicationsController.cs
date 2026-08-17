using Microsoft.AspNetCore.Mvc;
using Nerdklubben.Application.Interfaces;
using Nerdklubben.Domain.Entities;
using Nerdklubben.Infrastructure.Data;

namespace Nerdklubben.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApplicationsController : ControllerBase
{
    private readonly DataContext _context;
    private readonly IEmailService _emailService;

    public ApplicationsController(DataContext context, IEmailService emailService)
    {
        _context = context;
        _emailService = emailService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateApplication([FromBody] ApplicationEntity application)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        application.CreatedAt = DateTime.UtcNow;
        application.Status = "Pending";

        _context.Applications.Add(application);
        await _context.SaveChangesAsync();

        await _emailService.SendConfirmationEmailAsync(application.Email, application.FirstName);

        return Ok(new { message = "Ansökan har skickats framgångsrikt!" });
    }

    
}