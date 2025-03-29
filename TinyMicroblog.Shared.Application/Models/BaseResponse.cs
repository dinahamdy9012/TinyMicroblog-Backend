namespace TinyMicroblog.Shared.Application.Models;

public class BaseResponse
{
    public bool Result { get; set; }
    public string ErrorCode { get; set; } = null!;

    public BaseResponse()
    {

    }

    public BaseResponse Success()
    {
        Result = true;
        ErrorCode = string.Empty;
        return this;
    }


    public BaseResponse Failure(string errorCode)
    {
        Result = false;
        ErrorCode = errorCode;
        return this;
    }
}
