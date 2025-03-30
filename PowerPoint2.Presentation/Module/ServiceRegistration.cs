using PowerPoint2.Core.Interfaces;
using PowerPoint2.Core.Models;
using PowerPoint2.Infrastructure.Repositories;
using PowerPoint2.Infrastructure.Services;

namespace PowerPoint2.Presentation.Module;

public static class ServiceRegistration
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        _ = services.AddScoped<IEntityRepository<PresentationEntity>, EntityRepository<PresentationEntity>>();
        _ = services.AddScoped<IEntityRepository<Slide>, EntityRepository<Slide>>();
        _ = services.AddScoped<IPresentationService, PresentationService>();
        _ = services.AddScoped<PresentationHub>();
    }
}