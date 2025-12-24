using System.ComponentModel.DataAnnotations;

namespace NewsMVC.Models.News;

public class News
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    
    [Required]
    [MaxLength(100)]
    public required string Title { get; set; }
    
    [Required]
    [MaxLength(100)]
    public required string Content { get; set; }
    
    public List<NewsComment> Comments { get; set; } = [];
}