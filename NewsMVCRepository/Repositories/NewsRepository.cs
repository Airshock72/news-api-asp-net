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

    public async Task RemoveItem(News news)
    {
        _context.Remove(news);
        await _context.SaveChangesAsync();
    }
}