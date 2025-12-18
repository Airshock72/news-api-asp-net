using NewsMinimalApi;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

List<News> news = [];

app.MapPost("/News", (PostNewsRequest request) =>
{
    News newRequest = new()
    {
        Id = Guid.NewGuid(),
        Date = DateTime.Now,
        Title = request.Title,
        Content = request.Content
    };
    
    news.Add(newRequest);
    
    return Results.Ok(newRequest.Id);
});

app.MapGet("/News/{id}", (Guid id) =>
{
    News? foundItem = news.Find(x => x.Id == id);

    if (foundItem == null) 
        return Results.NotFound("ჩანაწერი ვერ მოიძებნა");
    
    return Results.Ok(new GetNewsResponse
    {
        Id = foundItem.Id,
        Content = foundItem.Content,
        Title = foundItem.Title,
        Date = foundItem.Date,
        Comments = foundItem.Comments
    });
});

app.MapGet("/News", () =>
{
    GetAllNewsResponse[] response = news.Select(x => new GetAllNewsResponse
    {
        Id = x.Id,
        Title = x.Title,
        Date = x.Date
    }).ToArray();
    
    return Results.Ok(response);
});

app.MapDelete("/News/{id}", (Guid id) =>
{
    News? foundItem = news.Find(x => x.Id == id);

    if (foundItem == null) return Results.NotFound();

    news.Remove(foundItem);

    return Results.Ok();

});

app.MapPut("/News/{id}", (Guid id, PutNewsRequest response) =>
{
    int foundItemIndex = news.FindIndex(x => x.Id == id);
    if (foundItemIndex == -1) return Results.NotFound();
    
    news[foundItemIndex].Title = response.Title;
    news[foundItemIndex].Content = response.Content;
    
    return Results.Ok();
});

app.MapPost("/News/{id}/Comment", (Guid id, NewsCommentRequest request) =>
{
    int foundNewsIndex = news.FindIndex(x => x.Id == id);
    if (foundNewsIndex == -1) return Results.NotFound();

    NewsComment newComment = new NewsComment
    {
        Id = Guid.NewGuid(),
        Text = request.Text
    };
    
    news[foundNewsIndex].Comments.Add(newComment);
    return Results.Ok(newComment.Id);
});

app.MapDelete("/News/{id}/Comment/{commentId}", (Guid id, Guid commentId) =>
{
    int foundNewsIndex = news.FindIndex(x => x.Id == id);
    if (foundNewsIndex == -1) 
        return Results.NotFound();
    
    int foundCommentIndex = news[foundNewsIndex].Comments.FindIndex(x => x.Id == commentId);
    if (foundCommentIndex == -1) 
        return Results.NotFound();
    
    news[foundCommentIndex].Comments.RemoveAt(foundCommentIndex);
    return Results.Ok();
});

app.UseHttpsRedirection();

app.Run();

namespace NewsMinimalApi
{
    public record PostNewsRequest
    {
        public required string Title { get; init; }
        public required string Content { get; init; }
    }

    public record GetNewsResponse
    {
        public DateTime? Date { get; init; }
        public Guid Id { get; init; }
        public required string Title { get; init; } 
        public required string Content { get; init; }
        public required List<NewsComment> Comments { get; init; }
    }

    public class News
    {
        public Guid Id { get; init; }
        public DateTime Date { get; init; }
        public required string Title { get; set; }
        public required string Content { get; set; }
        public List<NewsComment> Comments { get; init; } = [];
    }

    public class NewsComment
    {
        public Guid Id { get; init; }
        public required string Text { get; init; }
    }


    public record GetAllNewsResponse
    {
        public Guid Id { get; init; }
        public DateTime Date { get; init; }
        public required string Title { get; init; }
    }


    public record PutNewsRequest
    {
        public required string Title { get; init; }
        public required string Content { get; init; }
    }

    public record NewsCommentRequest
    {
        public required string Text { get; init; }
    }
}