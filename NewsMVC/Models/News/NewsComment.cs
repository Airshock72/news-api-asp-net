namespace NewsMVC.Models.News;

public class NewsComment
{
    public Guid Id { get; init; }
    public required string Text { get; init; }
}