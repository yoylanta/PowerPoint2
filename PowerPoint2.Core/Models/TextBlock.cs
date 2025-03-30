namespace PowerPoint2.Core.Models;

public class TextBlock
{
    public int Id { get; set; }

    public int SlideId { get; set; }

    public Slide Slide { get; set; }

    public string MarkdownText { get; set; }

    public int X { get; set; }

    public int Y { get; set; }

    public int Width { get; set; }

    public int Height { get; set; }
}