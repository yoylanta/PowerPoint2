namespace PowerPoint2.Core.Models;

public class Slide
{
    public int Id { get; set; }

    public int PresentationId { get; set; }

    public PresentationEntity Presentation { get; set; }

    public string SlideTitle { get; set; }

    public List<TextBlock> TextBlocks { get; set; } = [];
}
