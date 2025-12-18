namespace NewsMVC.Views.News.PostNews;

public record PostNewsRequest
{
    public required string Title { get; init; }
    public required string Content { get; init; }
}