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
                blogPost.OwnsMany(bp => bp.Categories);
            });

            // Configure owned collection for SocialPosts
            entity.OwnsMany(e => e.SocialPosts);

            // Add discriminator for derived types
            entity.HasDiscriminator<string>("EntryType")
                .HasValue<Entry>("Entry")
                .HasValue<BlogPost>("BlogPost");
        });

        // Configure BlogPost - it inherits from Entry so most config is there
        modelBuilder.Entity<BlogPost>();

        // Configure Category as a standalone entity
        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToContainer("Categories");
            entity.HasNoDiscriminator();
        });

        // Configure Tag as a standalone entity
        modelBuilder.Entity<Tag>(entity =>
        {
            entity.ToContainer("Tags");
            entity.HasNoDiscriminator();
        });

        // Configure SocialPost as owned entity (it doesn't inherit from BaseType)
        // It will be stored as part of Entry documents
    }
}
