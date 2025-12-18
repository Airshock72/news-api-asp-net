namespace NewsMVC.Views.News.PostNewsComment;

public record NewsCommentRequest
{
    public required string Text { get; init; }
}