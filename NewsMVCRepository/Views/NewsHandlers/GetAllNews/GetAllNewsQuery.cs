using MediatR;
using NewsMVCRepository.Common;

namespace NewsMVCRepository.Views.NewsHandlers.GetAllNews;

public class GetAllNewsQuery : IRequest<BaseResponse>;