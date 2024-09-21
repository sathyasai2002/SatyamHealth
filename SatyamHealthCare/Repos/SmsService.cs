using SatyamHealthCare.IRepos;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
namespace SatyamHealthCare.Repos
{
    public class SmsService : ISmsService
    {
        private readonly IConfiguration _configuration;

        public SmsService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendSmsAsync(string phoneNumber, string message)
        {
            var accountSid = _configuration["Twilio:AccountSid"];
            var authToken = _configuration["Twilio:AuthToken"];
            var fromNumber = _configuration["Twilio:PhoneNumber"];

            TwilioClient.Init(accountSid, authToken);

            var messageOptions = new CreateMessageOptions(new PhoneNumber(phoneNumber))
            {
                From = new PhoneNumber(fromNumber),
                Body = message
            };

            await MessageResource.CreateAsync(messageOptions);
        }
    }
}