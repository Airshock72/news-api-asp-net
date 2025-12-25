using MediatR;
using NewsMVCRepository.Common;

namespace NewsMVCRepository.Views.NewsHandlers.GetNews;

public class GetNewsQuery : IRequest<BaseResponse>
{
    public Guid Id { get; init; }
}