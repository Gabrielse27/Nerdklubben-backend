using Microsoft.AspNetCore.Mvc;
using Nerdklubben.Domain.Entities;
using Nerdklubben.Infrastructure.Data;

namespace Nerdklubben.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApplicationsController : ControllerBase
{
    private readonly DataContext _context;

    public ApplicationsController(DataContext context)
    {
        _context = context;
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

        return Ok(new { message = "Ansökan har skickats framgångsrikt!" });
    }
}