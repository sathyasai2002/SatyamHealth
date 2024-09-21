namespace SatyamHealthCare.Exceptions
{
    public class PrescribedTestNotFoundException : Exception
    {
        public PrescribedTestNotFoundException() : base("Prescribed test not found.")
        {
        }

        public PrescribedTestNotFoundException(string message) : base(message)
        {
        }

        public PrescribedTestNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
