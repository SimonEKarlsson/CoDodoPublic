namespace CoDodoApi.Entities
{
    public class ErrorDTO(string message, int code)
    {
        public string Message { get; set; } = message;
        public int Code { get; set; } = code;
    }
}
