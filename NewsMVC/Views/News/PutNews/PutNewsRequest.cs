namespace NewsMVC.Views.News.PutNews;

public record PutNewsRequest
{
    public required string Title { get; init; }
    public required string Content { get; init; }
}