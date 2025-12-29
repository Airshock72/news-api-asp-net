using MediatR;
using NewsMVCRepository.Common;
using NewsMVCRepository.Models.News;
using NewsMVCRepository.Repositories;

namespace NewsMVCRepository.Views.NewsHandlers.PostNewsComment;

public class PostNewsCommentCommandHandler: IRequestHandler<PostNewsCommentCommand, BaseResponse>
{
    private readonly NewsRepository _newsRepository;
    private readonly NewsCommentsRepository _newsCommentsRepository;
    
    public PostNewsCommentCommandHandler(NewsRepository newsRepository, NewsCommentsRepository newsCommentsRepository)
    {
        _newsRepository = newsRepository;
        _newsCommentsRepository = newsCommentsRepository;
    }

    public async Task<BaseResponse> Handle(PostNewsCommentCommand command, CancellationToken cancellationToken)
    {
        int foundNews = await _newsRepository.CountNews(command.Id);
        if (foundNews == 0) return BaseResponse.NotFound();
        
        NewsComment newComment = new NewsComment
        {
            Id = Guid.NewGuid(),
            Text = command.Text,
            NewsId = command.Id
        };
        
        _newsCommentsRepository.AddItem(newComment);
        await _newsRepository.SaveChanges(cancellationToken);
        
        return BaseResponse.Ok(newComment.Id);
    }
}