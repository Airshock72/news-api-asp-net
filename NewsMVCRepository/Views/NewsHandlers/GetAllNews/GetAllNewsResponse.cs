namespace NewsMVCRepository.Views.NewsHandlers.GetAllNews;

public record GetAllNewsResponse
{
    public Guid Id { get; init; }
    public DateTime Date { get; init; }
    public required string Title { get; init; }
}