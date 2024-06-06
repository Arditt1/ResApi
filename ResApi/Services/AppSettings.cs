// ReSharper disable InconsistentNaming
namespace ResApi.Services
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public int EInsureUniqueCompanyNumber { get; set; }
        public string EmailConfirmationLink { get; set; }
        public int EmailConfirmExpirationMinutes { get; set; }
        public bool IsEmailConfirmationMode { get; set; }
        public bool EnableMotorClaims { get; set; }
    }



    public class JwtOptions
    {
        public string SecretKey { get; set; }
        public int ExpiryMinutes { get; set; }
        public string Issuer { get; set; }
    }

}