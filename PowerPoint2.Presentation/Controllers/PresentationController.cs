using Microsoft.AspNetCore.Mvc;
using PowerPoint2.Core.Interfaces;

namespace PowerPoint2.Presentation.Controllers;

[Route("Presentation")]
public class PresentationController(IPresentationService presentationService) : Controller
{
    private readonly IPresentationService _presentationService = presentationService;

    [HttpGet("Create")]
    public async Task<IActionResult> Create(string nickname)
    {
        if (string.IsNullOrEmpty(nickname))
        {
            return BadRequest("Nickname is required");
        }

        var newPresentation = await _presentationService.CreatePresentationAsync("New Presentation", nickname);
        return Redirect($"/Presentation?presentationId={newPresentation.Id}&nickname={nickname}");
    }

    [HttpPost("UpdateTitle")]
    public async Task<IActionResult> UpdateTitle(int presentationId, string newTitle)
    {
        var presentation = await _presentationService.GetPresentationAsync(presentationId);
        if (presentation == null)
        {
            return NotFound();
        }

        // Update the title
        presentation.Title = newTitle;

        // Save the changes
        _presentationService.UpdatePresintation(presentation);

        return Ok(new { title = presentation.Title });
    }
}
