using System.ComponentModel.DataAnnotations;

namespace NewsMVCRepository.Models.News;

public class NewsComment
{
    public Guid Id { get; set; }
    [MaxLength(100)]
    public required string Text { get; set; }
    
    public Guid NewsId { get; set; }
    public News News { get; set; } = null!;
}