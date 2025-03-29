namespace TinyMicroblog.Shared.Application.Models;

public class DataResponse<T>
{
    public int Total { get; set; }
    public T? Data { get; set; }
    public string? ErrorCode { get; set; }

    public DataResponse<T> Success(T data, int total=0)
    {
        ErrorCode = string.Empty;
        Data = data;
        Total = total;
        return this;
    }

    public DataResponse<T?> Failure(string errorCode)
    {
        ErrorCode = errorCode;
        return this;
    }
}
