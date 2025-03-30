using Microsoft.AspNetCore.Mvc.RazorPages;
using PowerPoint2.Core.Interfaces;
using PowerPoint2.Core.Models;

namespace PowerPoint2.Presentation.Pages;

public class IndexModel(IPresentationService presentationService) : PageModel
{
    public IEnumerable<PresentationEntity> Presentations { get; set; } = [];

    public async Task OnGetAsync()
    {
        Presentations = await presentationService.GetAllPresentationsAsync();
    }
}