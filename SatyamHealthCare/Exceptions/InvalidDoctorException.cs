namespace SatyamHealthCare.Exceptions
{
    public class InvalidDoctorException : Exception
    {
        public InvalidDoctorException() : base("Invalid doctor information.")
        {
        }

        public InvalidDoctorException(string message) : base(message)
        {
        }

        public InvalidDoctorException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
