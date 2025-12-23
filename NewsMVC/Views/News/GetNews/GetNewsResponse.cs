namespace NewsMVC.Views.News.GetNews;

public record GetNewsResponse
{
    public DateTime? Date { get; init; }
    public Guid Id { get; init; }
    public required string Title { get; init; } 
    public required string Content { get; init; }
    public required List<GetNewsResponseComments> Comments { get; init; }
}

public record GetNewsResponseComments
{
    public Guid Id { get; init; }
    public required string Text { get; set; }
}