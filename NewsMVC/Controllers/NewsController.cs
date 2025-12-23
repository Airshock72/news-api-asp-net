using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsMVC.Data;
using NewsMVC.Models.News;
using NewsMVC.Views.News.GetAllNews;
using NewsMVC.Views.News.GetNews;
using NewsMVC.Views.News.PostNews;
using NewsMVC.Views.News.PostNewsComment;
using NewsMVC.Views.News.PutNews;

namespace NewsMVC.Controllers;

[ApiController]
[Route("[controller]")]
public class NewsController : ControllerBase
{
    private readonly TrainingDataContext _context;
    public NewsController(TrainingDataContext context) { _context = context; }
    
    [HttpGet] 
    [Route("")]
    [ProducesResponseType(typeof(GetAllNewsResponse[]), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllNews()
    {
        GetAllNewsResponse[] response = await _context.News.Select(x => new GetAllNewsResponse
        {
            Id = x.Id,
            Title = x.Title,
            Date = x.Date
        }).ToArrayAsync();
    
        return Ok(response);
    }
    
    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(typeof(GetNewsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetNews(Guid id)
    {
        News? foundItem = await _context.News
                .Include(x => x.Comments)
                .FirstOrDefaultAsync(x => x.Id == id);
    
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
                }).ToList()
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
    
        _context.News.Add(newRequest);
        await _context.SaveChangesAsync();
    
        return Ok(newRequest.Id);
    }
    
    [HttpDelete]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteNews(Guid id)
    {
        News? foundItem = await _context.News.FindAsync(id);
    
        if (foundItem == null) return NotFound();
    
        _context.News.Remove(foundItem);
        await _context.SaveChangesAsync();
    
        return Ok();
    }
    
    [HttpPut]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PutNews(Guid id, PutNewsRequest response)
    {
        News? foundItem = await _context.News.FindAsync(id);
        if (foundItem == null) return NotFound();
        
        foundItem.Title = response.Title;
        foundItem.Content = response.Content;
        
        await _context.SaveChangesAsync();
    
        return Ok();
    }
    
    [HttpPost]
    [Route("{id}/Comments")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PostNewsComment(Guid id, NewsCommentRequest request)
    {
        int foundNews = await _context.News.CountAsync(x => x.Id == id);
        if (foundNews == 0) return NotFound();
    
        NewsComment newComment = new NewsComment
        {
            Id = Guid.NewGuid(),
            Text = request.Text,
            NewsId = id
        };
        
        _context.NewsComments.Add(newComment);
        await _context.SaveChangesAsync();
        
        return Ok(newComment.Id);
    }
    
    // [HttpDelete]
    // [Route("{id}/Comments/{commentId}")0]
    // [ProducesResponseType(StatusCodes.Status200OK)]
    // [ProducesResponseType(StatusCodes.Status404NotFound)]
    // public IActionResult DeleteNewsComment(Guid id, Guid commentId)
    // {
    //     int foundNewsIndex = news.FindIndex(x => x.Id == id);
    //     if (foundNewsIndex == -1) 
    //         return NotFound();
    //
    //     int foundCommentIndex = news[foundNewsIndex].Comments.FindIndex(x => x.Id == commentId);
    //     if (foundCommentIndex == -1) 
    //         return NotFound();
    //
    //     news[foundCommentIndex].Comments.RemoveAt(foundCommentIndex);
    //     return Ok();
    // }
}