using Microsoft.EntityFrameworkCore;
using SocialManager.Data.Types;
using SocialManager.Data.Types.Blog;
using SocialManager.Data.Types.Social;

namespace SocialManager.Data.CosmosDb;

public class SocialManagerDbContext : DbContext
{
    public SocialManagerDbContext(DbContextOptions<SocialManagerDbContext> options)
        : base(options)
    {
    }

    public DbSet<Entry> Entries => Set<Entry>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Entry entity
        modelBuilder.Entity<Entry>(entity =>
        {
            entity.ToContainer("Entries");
            entity.HasPartitionKey(e => e.Id);
            entity.HasKey(e => e.Id);

            // Configure owned navigation for BlogPost
            entity.OwnsOne(e => e.BlogPost, blogPost =>
            {
                blogPost.OwnsMany(bp => bp.Tags);
            });

            // Configure owned collection for SocialPosts
            entity.OwnsMany(e => e.SocialPosts);

            // Add discriminator for derived types
            entity.HasDiscriminator<string>("EntryType")
                .HasValue<Entry>("Entry")
                .HasValue<BlogPost>("BlogPost");
        });
    }
}
