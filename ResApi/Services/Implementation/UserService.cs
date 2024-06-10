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

        public UserService(ILogger<UserService> logger, IUserRepository repository, IOptions<AppSettings> appSettings, IOptions<JwtOptions> jwtOptions)
        {
            _repository = repository;
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

            var result = await _repository.Authenticate(model);

            // return null if user not found
            if (result.Succeeded)
            {
                var extEmployeeResult = GetEmployeeProfile(result.Data.EmployeeID);

                if (extEmployeeResult.Succeeded && extEmployeeResult.Data != null)
                {
                    //check is user active in einsure
                    if (extEmployeeResult.Data.Status == true)
                    {
                        var token = GenerateJwtToken(result.Data);
                        //Set menus for user
                        response.Data = new AuthenticateUserDto(result.Data.EmployeeID, result.Data.Name, result.Data.Surname, result.Data.RoleID,
                                                     result.Data.Role, result.Data.Username, result.Data.Password,
                                                     result.Data.ContactInfo, result.Data.Orders, result.Data.AssignedTables,
                                                     result.Data.Status, token);
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
                    response.ErrorMessage = extEmployeeResult.ErrorMessage;
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

        public async Task<DataResponse<string>> Register(RegisterUserDto model)
        {
            var response = new DataResponse<string>() { Succeeded = false, Data = string.Empty };



            var checkIfUserExists = await _repository.CheckUserExists(model.Username);
            if (checkIfUserExists)
            {
                response.ErrorMessage = "Perdoruesi me usernamin: " + model.Username + " ekziston";
                return response;
            }

            try
            {
                Employee employee = new Employee();
                employee.Name = model.Name;
                employee.Surname = model.Surname;
                employee.RoleID = model.RoleID;
                employee.Role = model.Role;
                employee.Username = model.Username;
                employee.Password = model.Password;
                employee.ContactInfo = model.ContactInfo;
                employee.Status = model.Status;

                var entity = await _repository.AddEntity(employee); // Adding the user to context of users.
                if (entity != null)
                {
                    response.Succeeded = true;
                    response.Data = entity.Username;
                    return response;
                }
            }
            catch (Exception e)
            {
                response.ErrorMessage = "Per shkak te problemeve teknike nuk jemi ne gjendje te krijojme profilin.";
                _logger.LogError(e, $"CreateCustomerProfile: On adding user in customer portal db error {e.Message}");
            }

            return response;

        }

        public DataResponse<Employee> GetEmployeeProfile(int id)
        {
            DataResponse<Employee> response = new DataResponse<Employee> { Data = new Employee() };

            Employee obj = new Employee();
            try
            {
                obj.Load(id);

                if (obj.NewRecord)
                {
                    response.ResponseCode = EDataResponseCode.NoDataFound;
                    response.ErrorMessage = "Error";
                    return response;
                }
                // If profile is blocked, return just the status
                if (!obj.Status)
                {
                    response.Data.Status = obj.Status;
                    response.Succeeded = true;
                    response.ResponseCode = EDataResponseCode.Locked;
                    return response;
                }

                response.Data.Status = obj.Status;
                response.Data.Name = obj.Name;
                response.Data.Surname = obj.Surname;
                response.Data.RoleID = obj.RoleID;
                response.Data.Role = obj.Role;
                response.Data.Username = obj.Username;
                response.Data.Password = obj.Password;
                response.Data.ContactInfo = obj.ContactInfo;
                response.Data.Orders = obj.Orders;
                response.Data.AssignedTables = obj.AssignedTables;

                response.Succeeded = true;
                response.ResponseCode = EDataResponseCode.Success;
                return response;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"GetEmployeeProfile: Failed to retrieve employee profile from database. Error: {e.Message}");
                response.ErrorMessage = $"Failed to retrieve employee profile. Error: {e.Message}";
                return response;
            }
        }

        public async Task<DataResponse<bool>> UpdateEmployee(EmployeeDto model)
        {
            var response = new DataResponse<bool>
            {
                Succeeded = false,
                ErrorMessage = "Per shkak te arsyeve teknike nuk mund te perditesojme klientin"
            };

            //var resultUpdate = await _externalService.UpdateEmployee(model);

            //if (!resultUpdate.Succeeded)
            //{
            //    _logger.LogError("UpdateEmployee: When updating customer from EInsure failure occurred");
            //    return resultUpdate;
            //}

            try
            {
                Employee employee = await _repository.GetEntity(model.EmployeeID);
                employee.Name = model.Name;
                employee.Surname = model.Surname;
                employee.RoleID = model.RoleID;
                employee.Role = model.Role;
                employee.Username = model.Username;
                employee.Password = model.Password;
                employee.ContactInfo = model.ContactInfo;
                employee.Status = model.Status;

                var entity = await _repository.UpdateEntity(employee); // Update the user to context of users.
                if (entity != null)
                {
                    response.Succeeded = true;
                    response.Data = true;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"UpdateEmployee: Update customer failed when trying to save in Portal db {e.Message}");
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

            if (model.newPassword == user.Password)
            {
                if (model.newPassword == model.confirmNewPassword)
                {
                    var resultChange = await _repository.ChangePassword(user.Username, model.newPassword);
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

        public async Task<DataResponse<AuthenticateUserDto>> Login(string username)
        {
            var response = new DataResponse<AuthenticateUserDto>() { Data = new AuthenticateUserDto() };

            #region validation
            if (string.IsNullOrEmpty(username))
            {
                _logger.LogError("Authenticate: Empty input data");
                response.ResponseCode = EDataResponseCode.InvalidInputParameter;
                response.ErrorMessage = "Te dhenat hyrese jane bosh";
                return response;
            }
            #endregion

            var result = await _repository.Login(username);

            // return null if user not found
            if (result.Succeeded)
            {
                var extEmployeeResult = GetEmployeeProfile(result.Data.EmployeeID);

                if (extEmployeeResult.Succeeded && extEmployeeResult.Data != null)
                {
                    //check is user active in einsure
                    if (extEmployeeResult.Data.Status == true)
                    {
                        var token = GenerateJwtToken(result.Data);
                        //Set menus for user
                        response.Data = new AuthenticateUserDto(result.Data.EmployeeID, result.Data.Name, result.Data.Surname, result.Data.RoleID,
                                                     result.Data.Role, result.Data.Username, result.Data.Password,
                                                     result.Data.ContactInfo, result.Data.Orders, result.Data.AssignedTables,
                                                     result.Data.Status, token);
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
                    response.ErrorMessage = extEmployeeResult.ErrorMessage;
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

        private string GenerateJwtToken(Employee employee)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("EmployeeIDId", employee.EmployeeID.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                Issuer = _jwtOptions.Issuer,
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
        #endregion
    }
}
