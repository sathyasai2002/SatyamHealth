namespace SatyamHealthCare.IRepos
{
    public interface ISmsService
    {
        Task SendSmsAsync(string phoneNumber, string message);
    }
}
