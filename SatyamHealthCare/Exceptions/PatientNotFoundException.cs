
namespace SatyamHealthCare.Exceptions
{
    public class PatientNotFoundException : Exception
    {
        public PatientNotFoundException() : base("Patient not found.")
        {
        }

        public PatientNotFoundException(string message) : base(message)
        {
        }

        public PatientNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
