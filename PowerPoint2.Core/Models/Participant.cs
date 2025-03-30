namespace PowerPoint2.Core.Models;

public class Participant
{
    public int Id { get; set; }

    public int PresentationId { get; set; }

    public PresentationEntity Presentation { get; set; }

    public string Nickname { get; set; }

    public ParticipantRole Role { get; set; }
}
