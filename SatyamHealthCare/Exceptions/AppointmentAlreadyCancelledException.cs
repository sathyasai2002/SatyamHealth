namespace SatyamHealthCare.Exceptions
{
    public class AppointmentAlreadyCancelledException : Exception
    {
        public AppointmentAlreadyCancelledException() : base("Appointment has already been cancelled.")
        {
        }

        public AppointmentAlreadyCancelledException(string message) : base(message)
        {
        }

        public AppointmentAlreadyCancelledException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
