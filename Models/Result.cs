namespace SpeechGenerator.Models
{
    public class Result
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

        public static Result Sucess(string message = "", object data = null)
        {
            return new Result
            {
                Success = true,
                Message = message,
                Data = data
            };
        }

        public static Result Fail(string message = "", object data = null)
        {
            return new Result
            {
                Success = false,
                Message = message,
                Data = data
            };
        }
    }
}
