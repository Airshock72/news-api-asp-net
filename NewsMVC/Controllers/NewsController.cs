using Microsoft.AspNetCore.Mvc;
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
    private static List<News> news = new();
    
    [HttpGet]
    [Route("")]
    public IActionResult GetAllNews()
    {
        GetAllNewsResponse[] response = news.Select(x => new GetAllNewsResponse
        {
            Id = x.Id,
            Title = x.Title,
            Date = x.Date
        }).ToArray();
    
        return Ok(response);
    }
    
    [HttpGet]
    [Route("{id}")]
    public IActionResult GetNews(Guid id)
    {
        News? foundItem = news.Find(x => x.Id == id);

        if (foundItem == null) 
            return NotFound("ჩანაწერი ვერ მოიძებნა");
    
        return Ok(new GetNewsResponse
        {
            Id = foundItem.Id,
            Content = foundItem.Content,
            Title = foundItem.Title,
            Date = foundItem.Date,
            Comments = foundItem.Comments
        });
    }
    
    [HttpPost]
    [Route("")]
    public IActionResult PostNews(PostNewsRequest request)
    {
        News newRequest = new()
        {
            Id = Guid.NewGuid(),
            Date = DateTime.Now,
            Title = request.Title,
            Content = request.Content
        };
    
        news.Add(newRequest);
    
        return Ok(newRequest.Id);
    }

    [HttpDelete]
    [Route("{id}")]
    public IActionResult DeleteNews(Guid id)
    {
        News? foundItem = news.Find(x => x.Id == id);

        if (foundItem == null) return NotFound();

        news.Remove(foundItem);

        return Ok();
    }

    [HttpPut]
    [Route("{id}")]
    public IActionResult PutNews(Guid id, PutNewsRequest response)
    {
        int foundItemIndex = news.FindIndex(x => x.Id == id);
        if (foundItemIndex == -1) return NotFound();
    
        news[foundItemIndex].Title = response.Title;
        news[foundItemIndex].Content = response.Content;
    
        return Ok();
    }

    [HttpPost]
    [Route("{id}/Comments")]
    public IActionResult PostNewsComment(Guid id, NewsCommentRequest request)
    {
        int foundNewsIndex = news.FindIndex(x => x.Id == id);
        if (foundNewsIndex == -1) return NotFound();

        NewsComment newComment = new NewsComment
        {
            Id = Guid.NewGuid(),
            Text = request.Text
        };
    
        news[foundNewsIndex].Comments.Add(newComment);
        return Ok(newComment.Id);
    }

    [HttpDelete]
    [Route("{id}/Comments/{commentId}")]
    public IActionResult DeleteNewsComment(Guid id, Guid commentId)
    {
        int foundNewsIndex = news.FindIndex(x => x.Id == id);
        if (foundNewsIndex == -1) 
            return NotFound();
    
        int foundCommentIndex = news[foundNewsIndex].Comments.FindIndex(x => x.Id == commentId);
        if (foundCommentIndex == -1) 
            return NotFound();
    
        news[foundCommentIndex].Comments.RemoveAt(foundCommentIndex);
        return Ok();
    }
}