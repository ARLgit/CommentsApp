namespace CommentsAPI.Models
{
    public class Response
    {
        public string Status { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
    public class Response<T>
    {
        public string Status { get; set; } = string.Empty;
        public T? Value { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
