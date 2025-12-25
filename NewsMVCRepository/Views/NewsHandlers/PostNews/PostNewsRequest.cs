using System.ComponentModel.DataAnnotations;

namespace NewsMVCRepository.Views.News.PostNews;

public record PostNewsRequest
{
    [Required]
    [MaxLength(100)]
    public required string Title { get; init; }
    [Required]
    [MaxLength(1000)]
    public required string Content { get; init; }
}