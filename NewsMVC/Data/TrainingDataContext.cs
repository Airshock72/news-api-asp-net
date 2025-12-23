using Microsoft.EntityFrameworkCore;
using NewsMVC.Models.News;

namespace NewsMVC.Data;

public class TrainingDataContext : DbContext
{
    public TrainingDataContext(DbContextOptions<TrainingDataContext> options) : base(options) {}
    
    public DbSet<News> News { get; set; }
    public DbSet<NewsComment> NewsComments { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<News>()
            .HasMany(n => n.Comments)
            .WithOne(nc => nc.News)
            .HasForeignKey(n => n.NewsId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}