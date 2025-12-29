using MediatR;
using Microsoft.AspNetCore.Mvc;
using NewsMVCRepository.Common;
using NewsMVCRepository.Repositories;
using NewsMVCRepository.Views.NewsHandlers.DeleteNews;
using NewsMVCRepository.Views.NewsHandlers.GetAllNews;
using NewsMVCRepository.Views.NewsHandlers.GetNews;
using NewsMVCRepository.Views.NewsHandlers.PostNews;
using NewsMVCRepository.Views.NewsHandlers.PostNewsComment;
using NewsMVCRepository.Views.NewsHandlers.PutNews;
using GetAllNewsResponse = NewsMVCRepository.Views.NewsHandlers.GetAllNews.GetAllNewsResponse;
using GetNewsResponse = NewsMVCRepository.Views.NewsHandlers.GetNews.GetNewsResponse;
using News = NewsMVCRepository.Models.News.News;
using NewsComment = NewsMVCRepository.Models.News.NewsComment;

namespace NewsMVCRepository.Controllers;

[ApiController]
[Route("[controller]")]
public class NewsController : ControllerBase
{
    private readonly ISender _mediator;
    private readonly NewsRepository _newsRepository;
    private readonly NewsCommentsRepository _newsCommentsRepository;

    public NewsController(
        ISender mediator,   
        NewsRepository newsRepository,
        NewsCommentsRepository newsCommentsRepository
    )
    {
        _mediator = mediator;
        _newsRepository = newsRepository;
        _newsCommentsRepository = newsCommentsRepository;
    }
    
    [HttpGet] 
    [Route("")]
    [ProducesResponseType(typeof(GetAllNewsResponse[]), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllNews()
    {
        GetAllNewsQuery query = new();
        BaseResponse result = await _mediator.Send(query);
        return StatusCode(result.StatusCode, result.Data);
    }
    
    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(typeof(GetNewsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetNews(Guid id)
    {
        GetNewsQuery query = new() { Id = id };
        BaseResponse result = await _mediator.Send(query);
        return StatusCode(result.StatusCode, result.Data);
    }
    
    [HttpPost]
    [Route("")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<IActionResult> PostNews(PostNewsCommand command)
    {
        BaseResponse result = await _mediator.Send(command);
        return StatusCode(result.StatusCode, result.Data);
        
    }
    
    [HttpDelete]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteNews(Guid id)
    {
        DeleteNewsCommand command = new() { Id = id };
        BaseResponse result = await _mediator.Send(command);
        return StatusCode(result.StatusCode, result.Data);
        
    }
    
    [HttpPut]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PutNews(Guid id, PutNewsCommand command)
    {
        command.Id = id;
        BaseResponse result = await _mediator.Send(command);
        return StatusCode(result.StatusCode, result.Data);
    }
    
    [HttpPost]
    [Route("{id}/Comments")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PostNewsComment(Guid id, PostNewsCommentCommand request)
    {
        request.Id = id;
        BaseResponse result = await _mediator.Send(request);
        return StatusCode(result.StatusCode, result.Data);
    }
    
    [HttpDelete]
    [Route("{id}/Comments/{commentId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteNewsComment(Guid id, Guid commentId)
    {
        News? foundNews = await _newsRepository.GetItem(id);
        if (foundNews == null) return NotFound();
        
        NewsComment? foundComment = foundNews.Comments.Find(x => x.Id == commentId);
        if (foundComment == null) return NotFound();
        
        _newsCommentsRepository.RemoveItem(foundComment);
        await _newsRepository.SaveChanges();
        
        return Ok();
    }
}