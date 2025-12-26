using System.Text.Json.Serialization;
using MediatR;
using NewsMVCRepository.Common;

namespace NewsMVCRepository.Views.NewsHandlers.PutNews;

public class PutNewsCommand: IRequest<BaseResponse> 
{
    public required string Title { get; init; }
    public required string Content { get; init; }
    [JsonIgnore]
    public Guid Id { get; set; }
}