using SatyamHealthCare.IRepos;

namespace SatyamHealthCare.Repos
{
    public class NotificationService : INotificationService
    {
        private readonly IEmailService _emailService;
        private readonly ISmsService _smsService;

        public NotificationService(IEmailService emailService, ISmsService smsService)
        {
            _emailService = emailService;
            _smsService = smsService;
        }

        public async Task SendAppointmentConfirmationAsync(string email, string phoneNumber, string patientName, string doctorName, DateTime appointmentDate, TimeSpan appointmentTime)
        {
            var subject = "Appointment Confirmation";
            var formattedDateTime = $"{appointmentDate:yyyy-MM-dd} {appointmentTime:hh\\:mm}";  // Formatting date and time
            var emailBody = $"Dear {patientName},<br><br>Your appointment with Dr. {doctorName} has been successfully booked for {formattedDateTime}.<br><br>Best regards,<br>Satyam HealthCare";
            var smsBody = $"Your appointment with Dr. {doctorName} is confirmed for {formattedDateTime}. Thank you!";

            await _emailService.SendEmailAsync(email, subject, emailBody);
            await _smsService.SendSmsAsync(phoneNumber, smsBody);
        }

        public async Task SendAppointmentCancellationAsync(string email, string phoneNumber, string patientName, string doctorName, DateTime appointmentDate, TimeSpan appointmentTime)
        {
            var formattedDateTime = $"{appointmentDate:yyyy-MM-dd} {appointmentTime:hh\\:mm}";
            var emailSubject = "Appointment Cancellation Notification";
            var emailBody = $"Dear {patientName},<br><br>Your appointment with Dr. {doctorName} scheduled for {formattedDateTime} has been cancelled.<br><br>Best regards,<br>Satyam HealthCare";
            var smsMessage = $"Dear {patientName}, your appointment with Dr. {doctorName} on {formattedDateTime} has been cancelled.";

            await _emailService.SendEmailAsync(email, emailSubject, emailBody);
            await _smsService.SendSmsAsync(phoneNumber, smsMessage);
        }

        public async Task SendAppointmentRescheduleAsync(string email, string phoneNumber, string patientName, string doctorName, DateTime appointmentDate, TimeSpan appointmentTime)
        {
            var formattedDateTime = $"{appointmentDate:yyyy-MM-dd} {appointmentTime:hh\\:mm}";
            var emailSubject = "Appointment Reschedule Notification";
            var emailBody = $"Dear {patientName},<br><br>Your appointment with Dr. {doctorName} has been rescheduled to {formattedDateTime}.<br><br>Best regards,<br>Satyam HealthCare";
            var smsMessage = $"Dear {patientName}, your appointment with Dr. {doctorName} has been rescheduled to {formattedDateTime}.";

            await _emailService.SendEmailAsync(email, emailSubject, emailBody);
            await _smsService.SendSmsAsync(phoneNumber, smsMessage);
        }
    }











}


