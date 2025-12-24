using Microsoft.EntityFrameworkCore;
using NewsMVCRepository.Data;
using NewsMVCRepository.Models.News;
using NewsMVCRepository.Views.News.GetAllNews;

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

    public async Task<News[]> GetAll()
    {
        return await _context.News.ToArrayAsync();
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
}