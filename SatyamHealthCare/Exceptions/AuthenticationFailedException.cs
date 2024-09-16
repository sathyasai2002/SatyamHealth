namespace SatyamHealthCare.Exceptions
{
    public class AuthenticationFailedException:Exception
    {
        public AuthenticationFailedException() : base() { }

        public AuthenticationFailedException(string message) : base(message) { }

        public AuthenticationFailedException(string message, Exception innerException) : base(message, innerException) { }
    }
}
