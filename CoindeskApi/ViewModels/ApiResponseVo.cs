namespace CoindeskApi.ViewModels;

public class ApiResponseVo<T>
{
    /// <summary>
    /// 是否成功
    /// </summary>
    public bool IsSuccess { get; set; }
    
    /// <summary>
    /// 錯誤訊息
    /// </summary>
    public List<string> ErrorMessages { get; set; }
    
    /// <summary>
    /// 回傳資料
    /// </summary>
    public T Data { get; set; }

    public static ApiResponseVo<T> CreateSuccess(T data)
    {
        return new ApiResponseVo<T>
        {
            IsSuccess = true,
            Data = data
        };
    }

    public static ApiResponseVo<T> CreateFailure(List<string> errorMessages)
    {
        return new ApiResponseVo<T>
        {
            IsSuccess = false,
            ErrorMessages = errorMessages
        };
    }
}