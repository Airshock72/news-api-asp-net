namespace NewsMVCRepository.Common;

public record BaseResponse
{
    public int StatusCode { get; init; }
    public object? Data { get; init; }
    
    public BaseResponse(int statusCode, object? data)
    {
        StatusCode = statusCode;
        Data = data;
    }

    public static BaseResponse Ok(object? data)
    {
        return new(200, data);
    }
}