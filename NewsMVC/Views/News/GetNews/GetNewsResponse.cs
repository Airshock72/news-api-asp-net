using NewsMVC.Models.News;

namespace NewsMVC.Views.News.GetNews;

public record GetNewsResponse
{
    public DateTime? Date { get; init; }
    public Guid Id { get; init; }
    public required string Title { get; init; } 
    public required string Content { get; init; }
    public required List<NewsComment> Comments { get; init; }
}