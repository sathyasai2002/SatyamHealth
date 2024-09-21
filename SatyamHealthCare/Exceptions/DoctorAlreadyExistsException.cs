namespace SatyamHealthCare.Exceptions
{
    public class DoctorAlreadyExistsException : Exception
    {
        public DoctorAlreadyExistsException() : base("Doctor already exists.")
        {
        }

        public DoctorAlreadyExistsException(string message) : base(message)
        {
        }

        public DoctorAlreadyExistsException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
