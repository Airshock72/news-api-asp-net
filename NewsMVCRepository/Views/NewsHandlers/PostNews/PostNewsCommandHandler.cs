using MediatR;
using NewsMVCRepository.Common;
using NewsMVCRepository.Repositories;

namespace NewsMVCRepository.Views.NewsHandlers.PostNews;

public class PostNewsCommandHandler: IRequestHandler<PostNewsCommand, BaseResponse>
{
    private readonly NewsRepository _newsRepository;
    
    public PostNewsCommandHandler(NewsRepository newsRepository)
    {
        _newsRepository = newsRepository;
    }
    
    public async Task<BaseResponse> Handle(PostNewsCommand command, CancellationToken cancellationToken)
    {
        Models.News.News newRequest = new()
        {
            Date = DateTime.UtcNow,
            Title = command.Title,
            Content = command.Content
        };
        
        _newsRepository.AddItem(newRequest);
        await _newsRepository.SaveChanges();
        
        return BaseResponse.Ok(newRequest.Id);
    }
}