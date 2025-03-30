using Microsoft.AspNetCore.SignalR;
using PowerPoint2.Core.Interfaces;

namespace PowerPoint2.Infrastructure.Services;

public class PresentationHub(IPresentationService presentationService) : Hub
{
    public async Task JoinPresentation(int presentationId, string nickname)
    {
        string groupName = $"presentation-{presentationId}";
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        await Clients.Group(groupName).SendAsync("UserJoined", nickname);
    }

    public async Task UpdateTextBlock(int slideId, int textBlockId, string newMarkdownText)
    {
        bool success = await presentationService.UpdateTextBlockAsync(slideId, textBlockId, newMarkdownText);
        if (!success) return;

        string groupName = $"presentation-{slideId}";
        await Clients.Group(groupName).SendAsync("TextBlockUpdated", slideId, textBlockId, newMarkdownText);
    }
}
