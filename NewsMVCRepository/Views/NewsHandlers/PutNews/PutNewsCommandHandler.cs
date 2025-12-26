using MediatR;
using NewsMVCRepository.Common;
using NewsMVCRepository.Models.News;
using NewsMVCRepository.Repositories;

namespace NewsMVCRepository.Views.NewsHandlers.PutNews;

public class PutNewsCommandHandler: IRequestHandler<PutNewsCommand, BaseResponse>
{
    private readonly NewsRepository _newsRepository;

    public PutNewsCommandHandler(NewsRepository newsRepository)
    {
        _newsRepository = newsRepository;
    }

    public async Task<BaseResponse> Handle(PutNewsCommand command, CancellationToken cancellationToken)
    {
        News? foundItem = await _newsRepository.GetById(command.Id);
        if (foundItem == null) return BaseResponse.NotFound();
        
        foundItem.Title = command.Title;
        foundItem.Content = command.Content;
        
        await _newsRepository.SaveChanges(cancellationToken);
        
        return BaseResponse.Ok(command.Id);
    }
}