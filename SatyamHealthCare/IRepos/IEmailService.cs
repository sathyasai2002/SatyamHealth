namespace SatyamHealthCare.IRepos
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
    }
}
