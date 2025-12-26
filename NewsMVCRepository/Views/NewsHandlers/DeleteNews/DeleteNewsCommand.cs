using MediatR;
using NewsMVCRepository.Common;

namespace NewsMVCRepository.Views.NewsHandlers.DeleteNews;

public class DeleteNewsCommand: IRequest<BaseResponse>
{
    public Guid Id { get; init; }
}