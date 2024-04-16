namespace Swallow.Exceptions.CustomExceptions
{
    public class ForbidException : Exception
    {
        public ForbidException()
        {
        }

        public ForbidException(string message) : base(message)
        {
        }

        public ForbidException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
