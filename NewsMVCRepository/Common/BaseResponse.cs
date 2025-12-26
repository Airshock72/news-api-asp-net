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

    public static BaseResponse Ok(object? data = null)
    {
        return new(200, data);
    }
    
    public static BaseResponse NotFound(string? data = null)
    {
        return new(404, data);
    }
    
    public static BaseResponse NoContent()
    {
        return new(204, null);
    }
}