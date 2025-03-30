using Microsoft.EntityFrameworkCore;
using PowerPoint2.Core.Models;

namespace PowerPoint2.Infrastructure
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<PresentationEntity> Presentations { get; set; }
        public DbSet<Slide> Slides { get; set; }
        public DbSet<TextBlock> TextBlocks { get; set; }
        public DbSet<Participant> Participants { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}