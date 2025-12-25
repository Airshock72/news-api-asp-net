using Microsoft.EntityFrameworkCore;
using NewsMVCRepository.Data;
using NewsMVCRepository.Models.News;

namespace NewsMVCRepository.Repositories;

public class NewsRepository
{
    private readonly TrainingDataContext _context;
    public NewsRepository(TrainingDataContext context)
    {
        _context = context;
    }

    public async Task<News?> GetById(Guid id)
    {
        return await _context.News.FindAsync(id);
    }

    public void RemoveItem(News news)
    {
        _context.Remove(news);
    }

    public async Task<News[]> GetAll(CancellationToken cancellationToken = default)
    {
        return await _context.News.ToArrayAsync(cancellationToken);
    }

    public async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<News?> GetItem(Guid id)
    {
        return await _context.News
            .Include(x => x.Comments)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public void AddItem(News news)
    {
        _context.News.Add(news);
    }

    public async Task<int> CountNews(Guid id)
    {
        return await _context.News.CountAsync(x => x.Id == id);
    }
}