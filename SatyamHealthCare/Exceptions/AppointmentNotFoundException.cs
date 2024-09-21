namespace SatyamHealthCare.Exceptions
{
    public class AppointmentNotFoundException : Exception
    {
        public AppointmentNotFoundException() : base("Appointment not found.")
        {
        }

        public AppointmentNotFoundException(string message) : base(message)
        {
        }

        public AppointmentNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
