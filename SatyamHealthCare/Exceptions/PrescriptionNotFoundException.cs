namespace SatyamHealthCare.Exceptions
{
    public class PrescriptionNotFoundException : Exception
    {
        public PrescriptionNotFoundException() : base("Prescription not found.")
        {
        }

        public PrescriptionNotFoundException(string message) : base(message)
        {
        }

        public PrescriptionNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
