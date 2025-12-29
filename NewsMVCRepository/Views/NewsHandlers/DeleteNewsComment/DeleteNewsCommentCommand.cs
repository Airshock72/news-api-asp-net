using MediatR;
using NewsMVCRepository.Common;

namespace NewsMVCRepository.Views.NewsHandlers.DeleteNewsComment;

public class DeleteNewsCommentCommand: IRequest<BaseResponse>
{
    public Guid Id { get; init; }
    public Guid CommentId { get; init; }
}