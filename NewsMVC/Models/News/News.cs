namespace NewsMVC.Models.News;

public class News
{
    public Guid Id { get; init; }
    public DateTime Date { get; init; }
    public required string Title { get; set; }
    public required string Content { get; set; }
    public List<NewsComment> Comments { get; init; } = [];
}