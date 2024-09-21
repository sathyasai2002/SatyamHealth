namespace SatyamHealthCare.Exceptions
{
    public class DoctorNotFoundException : Exception
    {
        public DoctorNotFoundException() : base("Doctor not found.")
        {
        }

        public DoctorNotFoundException(string message) : base(message)
        {
        }

        public DoctorNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
