namespace SatyamHealthCare.Exceptions
{
    public class MedicalHistoryFileNotFoundException : Exception
    {
        public MedicalHistoryFileNotFoundException() : base("Medical history file not found.")
        {
        }

        public MedicalHistoryFileNotFoundException(string message) : base(message)
        {
        }

        public MedicalHistoryFileNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
