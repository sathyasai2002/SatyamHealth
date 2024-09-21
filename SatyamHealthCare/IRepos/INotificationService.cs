namespace SatyamHealthCare.IRepos
{
    public interface INotificationService
    {
        Task SendAppointmentConfirmationAsync(string email, string phoneNumber, string patientName, string doctorName, DateTime appointmentDate, TimeSpan appointmentTime);
        Task SendAppointmentCancellationAsync(string email, string phoneNumber, string patientName, string doctorName, DateTime appointmentDate, TimeSpan appointmentTime);
        Task SendAppointmentRescheduleAsync(string email, string phoneNumber, string patientName, string doctorName, DateTime appointmentDate, TimeSpan appointmentTime);
    }
}
