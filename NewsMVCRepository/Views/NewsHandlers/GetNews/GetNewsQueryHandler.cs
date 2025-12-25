using MediatR;
using NewsMVCRepository.Common;
using NewsMVCRepository.Repositories;
using NewsMVCRepository.Views.News.GetNews;

namespace NewsMVCRepository.Views.NewsHandlers.GetNews;

public class GetNewsQueryHandler: IRequestHandler<GetNewsQuery, BaseResponse>
{
    private readonly NewsRepository _newsRepository;

    public GetNewsQueryHandler(NewsRepository newsRepository)
    {
        _newsRepository = newsRepository;
    }

    public async Task<BaseResponse> Handle(GetNewsQuery request, CancellationToken cancellationToken)
    {
        Models.News.News? foundItem = await _newsRepository.GetItem(request.Id, cancellationToken);
    
        if (foundItem == null) 
            return BaseResponse.NotFound("ჩანაწერი ვერ მოიძებნა");
    
        return BaseResponse.Ok(new GetNewsResponse
        {
            Id = foundItem.Id,
            Content = foundItem.Content,
            Title = foundItem.Title,
            Date = foundItem.Date,
            Comments = foundItem.Comments
                .Select(x => new GetNewsResponseComments 
                { 
                    Id = x.Id, 
                    Text = x.Text 
                }).ToArray()
        });
    }
}