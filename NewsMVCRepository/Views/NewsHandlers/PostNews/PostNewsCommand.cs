using System.ComponentModel.DataAnnotations;
using MediatR;
using NewsMVCRepository.Common;

namespace NewsMVCRepository.Views.NewsHandlers.PostNews;

public class PostNewsCommand: IRequest<BaseResponse>
{
    [Required]
    [MaxLength(100)]
    public required string Title { get; init; }
    [Required]
    [MaxLength(1000)]
    public required string Content { get; init; }
}