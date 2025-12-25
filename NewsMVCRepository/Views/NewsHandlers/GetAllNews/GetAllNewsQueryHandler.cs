using MediatR;
using NewsMVCRepository.Common;
using NewsMVCRepository.Repositories;

namespace NewsMVCRepository.Views.NewsHandlers.GetAllNews;

public class GetAllNewsQueryHandler : IRequestHandler<GetAllNewsQuery, BaseResponse>
{
    private readonly NewsRepository _newsRepository;

    public GetAllNewsQueryHandler(NewsRepository newsRepository)
    {
        _newsRepository = newsRepository;
    }

    public async Task<BaseResponse> Handle(GetAllNewsQuery request, CancellationToken cancellationToken)
    {
        Models.News.News[] allNews = await _newsRepository.GetAll(cancellationToken);

        GetAllNewsResponse[] response = allNews.Select(x => new GetAllNewsResponse
        {
            Id = x.Id,
            Title = x.Title,
            Date = x.Date
        }).ToArray();
        
        return BaseResponse.Ok(response);
    }
}