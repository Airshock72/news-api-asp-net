using System.ComponentModel.DataAnnotations;

namespace NewsMVC.Models.News;

public class NewsComment
{
    public Guid Id { get; init; }
    [MaxLength(100)]
    public required string Text { get; init; }
}