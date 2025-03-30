using PowerPoint2.Core.Interfaces;
using PowerPoint2.Core.Models;

namespace PowerPoint2.Infrastructure.Services;

public class PresentationService(
    IEntityRepository<PresentationEntity> presentationRepository,
    IEntityRepository<Slide> slideRepository) : IPresentationService
{
    private readonly IEntityRepository<PresentationEntity> _presentationRepository = presentationRepository;
    private readonly IEntityRepository<Slide> _slideRepository = slideRepository;

    public async Task<PresentationEntity> GetPresentationAsync(int presentationId)
    {
        var presentation = await _presentationRepository.FirstOrDefaultAsyncWithIncludes(
            p => p.Id == presentationId,
            p => p.Slides,
            p => p.Participants
        );

        return presentation ?? throw new Exception("Presentation not found");
    }

    public async Task<IEnumerable<PresentationEntity>> GetAllPresentationsAsync()
    {
        return await _presentationRepository.GetAllAsync();
    }

    public void UpdatePresintation(PresentationEntity presentationEntity)
    {
        _presentationRepository.Update(presentationEntity);
    }

    public async Task<PresentationEntity> CreatePresentationAsync(string title, string creatorNickname)
    {
        var presentation = new PresentationEntity
        {
            Title = title,
            Participants =
            [
                new Participant { Nickname = creatorNickname, Role = ParticipantRole.Creator }
            ],
            Slides = [new Slide { SlideTitle = "Introduction" }]
        };

        await _presentationRepository.AddAsync(presentation);
        await _presentationRepository.SaveChangesAsync();
        return presentation;
    }

    public async Task SaveSlideAsync(int slideId, Dictionary<int, string> updatedTextBlocks)
    {
        var slide = await _slideRepository.GetByIdAsync(slideId);
        if (slide == null) throw new Exception("Slide not found");

        foreach (var (textBlockId, newMarkdownText) in updatedTextBlocks)
        {
            var textBlock = slide.TextBlocks.FirstOrDefault(tb => tb.Id == textBlockId);
            if (textBlock != null)
            {
                textBlock.MarkdownText = newMarkdownText;
            }
        }

        _slideRepository.Update(slide);
        await _slideRepository.SaveChangesAsync();
    }

    public async Task SaveSlideAsync(Slide slide)
    {
        _slideRepository.Update(slide);
        await _slideRepository.SaveChangesAsync();
    }

    public async Task<Slide?> GetSlideAsync(int slideId)
    {
        var slide = await _slideRepository.FirstOrDefaultAsyncWithIncludes(
            s => s.Id == slideId,
            s => s.TextBlocks
        );

        return slide;
    }

    public async Task<bool> UpdateTextBlockAsync(int slideId, int textBlockId, string newMarkdownText)
    {
        var slide = await _slideRepository.GetByIdAsync(slideId);
        if (slide == null) return false;

        var textBlock = slide.TextBlocks.FirstOrDefault(tb => tb.Id == textBlockId);
        if (textBlock == null) return false;

        textBlock.MarkdownText = newMarkdownText;
        _slideRepository.Update(slide);
        await _slideRepository.SaveChangesAsync();
        return true;
    }
}