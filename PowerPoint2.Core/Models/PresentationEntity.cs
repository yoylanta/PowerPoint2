namespace PowerPoint2.Core.Models;

public class PresentationEntity
{
    public int Id { get; set; }

    public string Title { get; set; }

    public List<Slide> Slides { get; set; } = [];

    public List<Participant> Participants { get; set; } = [];
}
