using ResApi.Services.Interface;
using ResApi.DataResponse;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading.Tasks;
using ResApi.Services;

namespace ResApi.Services
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly IExternalService _externalService;
        private readonly IConfiguration _configuration;
        private readonly AppSettings _appSettings;

        public EmailService(ILogger<EmailService> logger, IExternalService externalService, IConfiguration configuration, IOptions<AppSettings> appSettings)
        {
            _logger = logger;
            _externalService = externalService;
            _configuration = configuration;
            _appSettings = appSettings.Value;
        }

        public async Task<DataResponse<bool>> SendForgottenPasswordEmail(string email, string newPassword)
        {
            var response = new DataResponse<bool>
            {
                Data = false,
                Succeeded = false,
                ResponseCode = EDataResponseCode.GenericError,
                ErrorMessage = "Per shkak te problemeve teknike nuk jemi ne gjendje te dergojme email"//"Due to technical issues we are not able to send mail."
            };

            try
            {
                var data = new ExtMailData()
                {
                    Subject = $"PORTALI KLIENTIT",
                    Body = GetMailBodyForgottenPass(newPassword),
                    Receiver = email
                };

                return await _externalService.SendMail(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occured on sending email when resetting password for user");
                response.ResponseCode = EDataResponseCode.GenericError;
                return response;
            }
        }


    }
}
