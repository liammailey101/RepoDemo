namespace RepoDemo.Service
{
    public class ServiceResult<T>
    {
        public bool IsSuccess { get; set; }
        public bool IsFailure => !IsSuccess;
        public T? Data { get; set; }
        public List<string> Errors { get; set; } = new();

        public static ServiceResult<T> Success(T data)
        {
            return new ServiceResult<T> { IsSuccess = true, Data = data };
        }

        public static ServiceResult<T> Failure(List<string> errors)
        {
            return new ServiceResult<T> { IsSuccess = false, Errors = errors };
        }

        public static ServiceResult<T> Failure(string error)
        {
            return new ServiceResult<T> { IsSuccess = false, Errors = [error] };
        }
    }
}
