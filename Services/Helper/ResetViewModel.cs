namespace Services.Helper
{
    public class GetViewModel<T>
    {
        public T data { get; set; }
        public bool isSuccess { get; set; }
        public string message { get; set; }
        public int statusCode { get; set; }
    }
}
