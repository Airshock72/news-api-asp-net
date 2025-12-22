using Microsoft.EntityFrameworkCore;
using NewsMVC.Models.News;

namespace NewsMVC.Data;

public class TrainingDataContext : DbContext
{
    public TrainingDataContext(DbContextOptions<TrainingDataContext> options) : base(options) {}
    
    public DbSet<News> News { get; set; }
    public DbSet<NewsComment> NewsComments { get; set; }
}