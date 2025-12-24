using Microsoft.AspNetCore.Mvc;
using NewsMVCRepository.Models.News;
using NewsMVCRepository.Repositories;
using NewsMVCRepository.Views.News.GetAllNews;
using NewsMVCRepository.Views.News.GetNews;
using NewsMVCRepository.Views.News.PostNews;
using NewsMVCRepository.Views.News.PostNewsComment;
using NewsMVCRepository.Views.News.PutNews;

namespace NewsMVCRepository.Controllers;

[ApiController]
[Route("[controller]")]
public class NewsController : ControllerBase
{
    private readonly NewsRepository _newsRepository;

    public NewsController(NewsRepository newsRepository) { _newsRepository = newsRepository; }
    
    [HttpGet] 
    [Route("")]
    [ProducesResponseType(typeof(GetAllNewsResponse[]), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllNews()
    {
        News[] allNews = await _newsRepository.GetAll();

        GetAllNewsResponse[] response = allNews.Select(x => new GetAllNewsResponse
        {
            Id = x.Id,
            Title = x.Title,
            Date = x.Date
        }).ToArray();
    
        return Ok(response);
    }
    
    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(typeof(GetNewsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetNews(Guid id)
    {
        News? foundItem = await _newsRepository.GetItem(id);
    
        if (foundItem == null) 
            return NotFound("ჩანაწერი ვერ მოიძებნა");
    
        return Ok(new GetNewsResponse
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
    
    [HttpPost]
    [Route("")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<IActionResult> PostNews(PostNewsRequest request)
    {
        News newRequest = new()
        {
            Date = DateTime.UtcNow,
            Title = request.Title,
            Content = request.Content
        };

        _newsRepository.AddItem(newRequest);
        await _newsRepository.SaveChanges();
    
        return Ok(newRequest.Id);
    }
    
    [HttpDelete]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteNews(Guid id)
    {
        News? foundItem = await _newsRepository.GetById(id);
    
        if (foundItem == null) return NotFound();
    
        _newsRepository.RemoveItem(foundItem);
        await _newsRepository.SaveChanges();
    
        return Ok();
    }
    
    // [HttpPut]
    // [Route("{id}")]
    // [ProducesResponseType(StatusCodes.Status200OK)]
    // [ProducesResponseType(StatusCodes.Status404NotFound)]
    // public async Task<IActionResult> PutNews(Guid id, PutNewsRequest response)
    // {
    //     News? foundItem = await _context.News.FindAsync(id);
    //     if (foundItem == null) return NotFound();
    //     
    //     foundItem.Title = response.Title;
    //     foundItem.Content = response.Content;
    //     
    //     await _context.SaveChangesAsync();
    //
    //     return Ok();
    // }
    
    // [HttpPost]
    // [Route("{id}/Comments")]
    // [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    // [ProducesResponseType(StatusCodes.Status404NotFound)]
    // public async Task<IActionResult> PostNewsComment(Guid id, NewsCommentRequest request)
    // {
    //     int foundNews = await _context.News.CountAsync(x => x.Id == id);
    //     if (foundNews == 0) return NotFound();
    //
    //     NewsComment newComment = new NewsComment
    //     {
    //         Id = Guid.NewGuid(),
    //         Text = request.Text,
    //         NewsId = id
    //     };
    //     
    //     _context.NewsComments.Add(newComment);
    //     await _context.SaveChangesAsync();
    //     
    //     return Ok(newComment.Id);
    // }
    
    // [HttpDelete]
    // [Route("{id}/Comments/{commentId}")]
    // [ProducesResponseType(StatusCodes.Status200OK)]
    // [ProducesResponseType(StatusCodes.Status404NotFound)]
    // public async Task<IActionResult> DeleteNewsComment(Guid id, Guid commentId)
    // {
    //     News? foundNews = await _context.News
    //         .Include(x => x.Comments)
    //         .FirstOrDefaultAsync(x => x.Id == id);
    //     if (foundNews == null) return NotFound();
    //     
    //     NewsComment? foundComment = foundNews.Comments.Find(x => x.Id == commentId);
    //     if (foundComment == null) return NotFound();
    //     
    //     _context.NewsComments.Remove(foundComment);
    //     await _context.SaveChangesAsync();
    //     
    //     return Ok();
    // }
}