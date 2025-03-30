using Microsoft.AspNetCore.Mvc;
using PowerPoint2.Core.DTOs;
using PowerPoint2.Core.Interfaces;

namespace PowerPoint2.Presentation.Controllers;

[Route("api/[controller]")]
public class SlidesController(IPresentationService presentationService) : Controller
{
    private readonly IPresentationService _presentationService = presentationService;

    // Fetch slide by ID
    [HttpGet("{slideId}")]
    public async Task<IActionResult> GetSlideAsync(int slideId)
    {
        var slide = await _presentationService.GetSlideAsync(slideId);
        if (slide == null)
        {
            return NotFound(); // Return 404 if slide is not found
        }

        // Assuming you want to return the HTML content of the slide
        return Ok(new { html = string.Join("\n", slide.TextBlocks.Select(tb => tb.MarkdownText)) });
    }

    [HttpPut("{slideId}/content")]
    public async Task<IActionResult> UpdateSlideContent(int slideId, [FromBody] SlideContentUpdateRequest request)
    {
        var slide = await _presentationService.GetSlideAsync(slideId);
        if (slide == null)
        {
            return NotFound();
        }

        // Handle text blocks and update each one
        var updatedTextBlocks = new Dictionary<int, string>();

        foreach (var textBlock in slide.TextBlocks)
        {
            if (request.TextBlocks.ContainsKey(textBlock.Id))
            {
                textBlock.MarkdownText = request.TextBlocks[textBlock.Id];  // Update the text block
                updatedTextBlocks[textBlock.Id] = textBlock.MarkdownText;
            }
        }

        // Save changes to the slide and text blocks
        await _presentationService.SaveSlideAsync(slide);

        return Ok(new { success = true });
    }

}
