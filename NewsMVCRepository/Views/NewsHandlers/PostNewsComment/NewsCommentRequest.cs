using MediatR;
using NewsMVCRepository.Common;

namespace NewsMVCRepository.Views.NewsHandlers.PostNewsComment;

public record NewsCommentRequest : IRequest<BaseResponse>
{
    public required string Text { get; init; }
}