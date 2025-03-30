using PowerPoint2.Core.Models;

namespace PowerPoint2.Core.Interfaces;

public interface IPresentationService
{
    Task<PresentationEntity> GetPresentationAsync(int presentationId);

    Task<IEnumerable<PresentationEntity>> GetAllPresentationsAsync();

    Task<PresentationEntity> CreatePresentationAsync(string title, string creatorNickname);

    Task<Slide?> GetSlideAsync(int slideId);

    Task<bool> UpdateTextBlockAsync(int slideId, int textBlockId, string newMarkdownText);

    Task SaveSlideAsync(int slideId, Dictionary<int, string> updatedTextBlocks);

    Task SaveSlideAsync(Slide slide);

    void UpdatePresintation(PresentationEntity presentationEntity);
}
