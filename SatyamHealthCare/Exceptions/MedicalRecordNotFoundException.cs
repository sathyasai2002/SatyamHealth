namespace SatyamHealthCare.Exceptions
{
    public class MedicalRecordNotFoundException : Exception
    {
        public MedicalRecordNotFoundException() : base("Medical record not found.")
        {
        }

        public MedicalRecordNotFoundException(string message) : base(message)
        {
        }

        public MedicalRecordNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
