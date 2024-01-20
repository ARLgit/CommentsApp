namespace CommentsAPI.Models
{
    public class Response
    {
        public bool Status { get; set; } = false;
        public string Message { get; set; } = string.Empty;
    }
    public class Response<T>
    {
        public bool Status { get; set; } = false;
        public string Message { get; set; } = string.Empty;
        public T? Value { get; set; }
    }
}
