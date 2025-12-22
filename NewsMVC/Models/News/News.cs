using System.ComponentModel.DataAnnotations;

namespace NewsMVC.Models.News;

public class News
{
    public Guid Id { get; init; }
    public DateTime Date { get; init; }
    [Required]
    [MaxLength(100)]
    public required string Title { get; set; }
    [Required]
    [MaxLength(100)]
    public required string Content { get; set; }
    public List<NewsComment> Comments { get; init; } = [];
}