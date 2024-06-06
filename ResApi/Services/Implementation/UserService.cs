using ResApi.Models;
using ResApi.Implementation;
using ResApi.Services.Interface;
using ResApi.DataResponse;
using ResApi.Dtos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ResApi.Services;

namespace ResApi.Implementation
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly AppSettings _appSettings;
        private readonly JwtOptions _jwtOptions;
        private readonly IUserRepository _repository;
        private readonly IExternalService _externalService;
        private readonly IEmailService _emailService;

        public UserService(ILogger<UserService> logger, IUserRepository repository, IExternalService externalService, IEmailService emailService, IOptions<AppSettings> appSettings, IOptions<JwtOptions> jwtOptions)
        {
            _repository = repository;
            _externalService = externalService;
            _emailService = emailService;
            _appSettings = appSettings.Value;
            _jwtOptions = jwtOptions.Value;
            _logger = logger;
        }

        #region PubliMethods

        public async Task<DataResponse<AuthenticateUserDto>> Authenticate(AuthenticateUserDto model)
        {
            var response = new DataResponse<AuthenticateUserDto>() { Data = new AuthenticateUserDto() };

            #region validation
            if (model == null)
            {
                _logger.LogError("Authenticate: Empty input data");
                response.ResponseCode = EDataResponseCode.NoDataFound;
                response.ErrorMessage = "Te dhenat hyrese jane bosh";
                return response;
            }
            #endregion

            var result = await _repository.Authenticate(model, _appSettings.IsEmailConfirmationMode);

            // return null if user not found
            if (result.Succeeded)
            {
                var extCustomerResult = await _externalService.GetCustomerProfile(result.Data.CustomerId);

                if (extCustomerResult.Succeeded && extCustomerResult.Data != null)
                {
                    //check is user active in einsure
                    if (extCustomerResult.Data.Status == "2")
                    {
                        var token = GenerateJwtToken(result.Data);
                        //Set menus for user
                        response.Data = new AuthenticateUserDto(result.Data.Id, result.Data.TipKind, extCustomerResult.Data.Nipt, extCustomerResult.Data.Clientno,
                            result.Data.Name, result.Data.Surname, result.Data.Email,
                            result.Data.Gender, result.Data.Mob, result.Data.Address, result.Data.Place, result.Data.Birthdate,
                            token, result.Data.CustomerId, extCustomerResult.Data.Notifications);
                        response.Succeeded = true;
                        return response;
                    }
                    else
                    {
                        _logger.LogError("Authenticate: User is not activated in eInsure");
                        response.Succeeded = false;
                        response.Data = null;
                        response.ErrorMessage = "Perdoruesi nuk eshte aktiv. Hyrja eshte e ndaluar";// "User is not active, login is forbidden!";
                        return response;
                    }
                }
                else
                {
                    _logger.LogError("Authenticate: couldn't get customer profile from eInsure");
                    response.Succeeded = false;
                    response.Data = null;
                    response.ErrorMessage = extCustomerResult.ErrorMessage;
                    response.ResponseCode = EDataResponseCode.NoDataFound;
                    return response;
                }
            }

            // authentication successful so generate jwt token
            response.ErrorMessage = result.ErrorMessage;
            response.Succeeded = result.Succeeded;
            response.ResponseCode = result.ResponseCode;
            return response;
        }


        public async Task<DefOperator> GetById(int id)
        {
            return await _repository.GetEntity(id);
        }


        public async Task<DataResponse<bool>> UpdateCustomer(CustomerDto model)
        {
            var response = new DataResponse<bool>
            {
                Succeeded = false,
                ErrorMessage = "Per shkak te arsyeve teknike nuk mund te perditesojme klientin"
            };

            var resultUpdate = await _externalService.UpdateCustomer(model);

            if (!resultUpdate.Succeeded)
            {
                _logger.LogError("UpdateCustomer: When updating customer from EInsure failure occurred");
                return resultUpdate;
            }

            try
            {

                DefOperator user = await _repository.GetEntity(model.OperatorId);
                user.TipKind = model.Tipkind;
                user.Email = model.Email;
                user.Birthdate = model.Birthdate;
                user.Gender = model.Sex;
                user.ClientId = model.Clientno;
                user.Mob = model.Mob;
                user.Name = model.Title;
                user.Surname = model.Lastname;
                user.Place = model.Place;
                user.Address = model.Address;
                user.TipKind = model.Tipkind;
                user.UpdateOn = DateTime.Now;

                var entity = await _repository.UpdateEntity(user); // Update the user to context of users.
                if (entity != null)
                {
                    response.Succeeded = true;
                    response.Data = true;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"UpdateCustomer: Update customer failed when trying to save in Portal db {e.Message}");
                response.ErrorMessage = "Per shkak te problemeve teknike nuk mund te perditesojme profilin";
            }

            return response;
        }

        public async Task<DataResponse<bool>> ChangePassword(ChangePasswordDto model)
        {
            var response = new DataResponse<bool>
            {
                Succeeded = false,
                ErrorMessage = "Per shkak te arsyeve teknike nuk mund te perditesojme klientin"
            };

            var user = await _repository.GetEntity(model.Id);

            if (_repository.VerifyPassword(model.oldPassword, user.PasswordHash, user.PasswordSalt))
            {
                if (model.newPassword == model.confirmNewPassword)
                {
                    var resultChange = await _repository.ChangePassword(user.Email, model.newPassword);
                    return resultChange;
                }
                else
                {
                    _logger.LogError("ChangePassword: Inputs for confirm and new don't match");
                    response.ErrorMessage = "Fjalekalimet e futura nuk perputhen";
                }
            }
            else
            {
                _logger.LogError("ChangePassword: Old password is not correct");
                response.ErrorMessage = "Fjalekalimi I vjeter nuk eshte I sakte";
            }

            return response;
        }

        public async Task<DataResponse<ExtCustomerProfile>> GetCustomer(Guid id)
        {
            return await _externalService.GetCustomerProfile(id);
        }

        public async Task<DataResponse<bool>> ConfirmEmail(string token)
        {
            var response = new DataResponse<bool> { Data = false, Succeeded = false };

            try
            {
                SecurityToken validatedToken = null;
                DefOperator user;

                try
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));

                    tokenHandler.ValidateToken(token, new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = key,
                        ValidIssuer = _jwtOptions.Issuer,
                        ValidateAudience = false,
                        // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                        ClockSkew = TimeSpan.Zero
                    }, out validatedToken);

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "CreateCustomerProfile: Token validation failed");
                    response.ErrorMessage = "Shenja ka skaduar, vërtetimi dështoi. Ju lutemi regjistrohuni përsëri!"; //User does not exist
                }

                var jwtToken = (JwtSecurityToken)validatedToken;

                if (jwtToken == null)
                {
                    return response;
                }

                try
                {
                    var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);
                    user = await _repository.GetEntity(userId);
                    if (user.ConfirmedMail)
                    {
                        response.Action = "1";
                        response.Succeeded = true;
                        return response;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "CreateCustomerProfile: Get entity failed (user does not exist)");
                    response.ErrorMessage = "Perdoruesi nuk ekziston"; //User does not exist

                    return response;
                }

                var model = new RegisterUserDto
                {
                    Email = user.Email,
                    Birthdate = user.Birthdate,
                    Sex = user.Gender,
                    ClientNo = user.ClientId,
                    Mob = user.Mob,
                    Title = user.Name,
                    LastName = user.Surname,
                    Place = user.Place,
                    Address = user.Address,
                    TipKind = user.TipKind,
                    CreatedOn = user.CreatedOn,
                    UpdateOn = user.UpdateOn,
                    Nipt = user.ClientId
                };

                var resultCreate = await _externalService.CreateCustomerProfile(model);

                if (!resultCreate.Succeeded)
                {
                    _logger.LogError("CreateCustomerProfile: Failed creation of customer profile in EInsure with error " + resultCreate.ErrorMessage);
                    response.ResponseCode = resultCreate.ResponseCode;
                    response.ErrorMessage = resultCreate.ErrorMessage;

                    return response;
                }

                user.ConfirmedMail = true;
                user.CustomerId = resultCreate.Data;

                var entity = await _repository.UpdateEntity(user);
                if (entity != null)
                {
                    response.Succeeded = true;
                    response.Data = true;
                }

                return response;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Lidhja e konfirmimit nuk është më e vlefshme"; //Confirmation link is no longer valid
                _logger.LogError(ex, "Error occurred on email confirmation");

                return response;
            }
        }

        public async Task<DataResponse<AuthenticateUserDto>> Login(string email)
        {
            var response = new DataResponse<AuthenticateUserDto>() { Data = new AuthenticateUserDto() };

            #region validation
            if (string.IsNullOrEmpty(email))
            {
                _logger.LogError("Authenticate: Empty input data");
                response.ResponseCode = EDataResponseCode.InvalidInputParameter;
                response.ErrorMessage = "Te dhenat hyrese jane bosh";
                return response;
            }
            #endregion

            var result = await _repository.Login(email);

            // return null if user not found
            if (result.Succeeded)
            {
                var extCustomerResult = await _externalService.GetCustomerProfile(result.Data.CustomerId);

                if (extCustomerResult.Succeeded && extCustomerResult.Data != null)
                {
                    //check is user active in einsure
                    if (extCustomerResult.Data.Status == "2")
                    {
                        var token = GenerateJwtToken(result.Data);
                        //Set menus for user
                        response.Data = new AuthenticateUserDto(result.Data.Id, result.Data.TipKind, extCustomerResult.Data.Nipt, extCustomerResult.Data.Clientno,
                            result.Data.Name, result.Data.Surname, result.Data.Email,
                            result.Data.Gender, result.Data.Mob, result.Data.Address, result.Data.Place, result.Data.Birthdate,
                            token, result.Data.CustomerId,  extCustomerResult.Data.Notifications);
                        response.Succeeded = true;
                        return response;
                    }
                    else
                    {
                        _logger.LogError("Login: User is not activated in eInsure");
                        response.Succeeded = false;
                        response.Data = null;
                        response.ErrorMessage = "Perdoruesi nuk eshte aktiv. Hyrja eshte e ndaluar";// "User is not active, login is forbidden!";
                        return response;
                    }
                }
                else
                {
                    _logger.LogError("Login: couldn't get customer profile from eInsure");
                    response.Succeeded = false;
                    response.Data = null;
                    response.ErrorMessage = extCustomerResult.ErrorMessage;
                    response.ResponseCode = EDataResponseCode.NoDataFound;
                    return response;
                }
            }

            // authentication successful so generate jwt token
            response.ErrorMessage = result.ErrorMessage;
            response.Succeeded = result.Succeeded;
            response.ResponseCode = result.ResponseCode;
            return response;
        }



        #endregion

        #region PrivateMethods

        private string GenerateJwtToken(DefOperator user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                Issuer = _jwtOptions.Issuer,
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        private string GenerateEmailConfirmationJwtToken(DefOperator user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddMinutes(_appSettings.EmailConfirmExpirationMinutes),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature),
                Issuer = _jwtOptions.Issuer,
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private static string GeneratePassword()
        {
            const string LOWER_CASE = "abcdefghijklmnopqursuvwxyz";
            const string NUMBERS = "123456789";
            const int passwordLength = 8;

            char[] _password = new char[passwordLength];
            string charSet = "";
            Random _random = new Random();
            int counter;

            charSet += LOWER_CASE;
            charSet += NUMBERS;

            for (counter = 0; counter < passwordLength; counter++)
            {
                _password[counter] = charSet[_random.Next(charSet.Length - 1)];
            }

            return String.Join(null, _password);
        }

        private static string GenerateVerificationCode()
        {
            Random generator = new Random();

            return generator.Next(0, 1000000).ToString("D6");
        }

        #endregion
    }
}
