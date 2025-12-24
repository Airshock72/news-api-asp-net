using NewsMVCRepository.Data;
using NewsMVCRepository.Models.News;

namespace NewsMVCRepository.Repositories;

public class NewsCommentsRepository
{
    private readonly TrainingDataContext _context;

    public NewsCommentsRepository(TrainingDataContext context)
    {
        _context = context;
    }

    public void RemoveItem(NewsComment newsComment)
    {
        _context.NewsComments.Remove(newsComment);
    }

    public void AddItem(NewsComment newsComment)
    {
        _context.NewsComments.Add(newsComment);
    }
}