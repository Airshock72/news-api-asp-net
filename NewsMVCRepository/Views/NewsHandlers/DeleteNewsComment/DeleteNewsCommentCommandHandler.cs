using MediatR;
using NewsMVCRepository.Common;
using NewsMVCRepository.Models.News;
using NewsMVCRepository.Repositories;

namespace NewsMVCRepository.Views.NewsHandlers.DeleteNewsComment;

public class DeleteNewsCommentCommandHandler: IRequestHandler<DeleteNewsCommentCommand, BaseResponse>
{
    private readonly NewsRepository _newsRepository;
    private readonly NewsCommentsRepository _newsCommentsRepository;

    public DeleteNewsCommentCommandHandler(NewsRepository newsRepository, NewsCommentsRepository newsCommentsRepository)
    {
        _newsRepository = newsRepository;
        _newsCommentsRepository = newsCommentsRepository;
    }
    
    public async Task<BaseResponse> Handle(DeleteNewsCommentCommand command, CancellationToken cancellationToken)
    {
        News? foundNews = await _newsRepository.GetItem(command.Id, cancellationToken);
        if (foundNews == null) return BaseResponse.NotFound();
        
        NewsComment? foundComment = foundNews.Comments.Find(x => x.Id == command.CommentId);
        if (foundComment == null) return BaseResponse.NotFound();
        
        _newsCommentsRepository.RemoveItem(foundComment);
        await _newsRepository.SaveChanges(cancellationToken);
        
        return BaseResponse.Ok();
    }
}