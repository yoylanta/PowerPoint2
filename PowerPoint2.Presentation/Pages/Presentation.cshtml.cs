using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PowerPoint2.Core.Interfaces;
using PowerPoint2.Core.Models;

namespace PowerPoint2.Presentation.Pages;

public class PresentationModel(IPresentationService presentationService) : PageModel
{
    public PresentationEntity Presentation { get; set; } = default!;

    // Nickname is passed as query parameter.
    [BindProperty(SupportsGet = true)]
    public string Nickname { get; set; } = string.Empty;

    // Flag to check if current user is the creator.
    public bool IsCreator { get; set; }
    // For demo, we assume the first slide is active.
    public int CurrentSlideId { get; set; }


    public async Task<IActionResult> OnGetAsync(int presentationId, string nickname)
    {
        if (string.IsNullOrWhiteSpace(nickname))
        {
            // Redirect to index if no nickname is provided.
            return RedirectToPage("Index");
        }

        Nickname = nickname;
        Presentation = await presentationService.GetPresentationAsync(presentationId);
        if (Presentation == null)
        {
            return NotFound();
        }

        // Example: assume the presentation creator is the first participant.
        IsCreator = Presentation.Participants.Any() &&
                    Presentation.Participants.First().Nickname == Nickname;

        // Set current slide id – default to first slide if available.
        CurrentSlideId = Presentation.Slides.FirstOrDefault()?.Id ?? 0;

        return Page();
    }
}
