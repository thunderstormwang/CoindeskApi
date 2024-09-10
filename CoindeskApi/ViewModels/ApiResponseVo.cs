namespace CoindeskApi.ViewModels;

public class ApiResponseVo<T>
{
    public bool IsSuccess { get; set; }
    
    public List<string> ErrorMessages { get; set; }
    
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