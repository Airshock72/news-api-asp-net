using System.Text.Json.Serialization;
using MediatR;
using NewsMVCRepository.Common;

namespace NewsMVCRepository.Views.NewsHandlers.PostNewsComment;

public class PostNewsCommentCommand: IRequest<BaseResponse>
{
    public required string Text { get; init; }
    [JsonIgnore]
    public Guid Id { get; set; }
}