using MediatR;
using NewsMVCRepository.Common;
using NewsMVCRepository.Repositories;

namespace NewsMVCRepository.Views.NewsHandlers.DeleteNews;

public class DeleteNewsCommandHandler: IRequestHandler<DeleteNewsCommand, BaseResponse>
{
    private readonly NewsRepository _newsRepository;

    public DeleteNewsCommandHandler(NewsRepository newsRepository)
    {
        _newsRepository = newsRepository;
    }

    public async Task<BaseResponse> Handle(DeleteNewsCommand command, CancellationToken cancellationToken)
    {
        Models.News.News? foundItem = await _newsRepository.GetById(command.Id);
    
        if (foundItem == null) return BaseResponse.NotFound();
    
        _newsRepository.RemoveItem(foundItem);
        await _newsRepository.SaveChanges();
    
        return BaseResponse.NoContent();
    }
}