using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ResApi.DataResponse;
using ResApi.DTA.Intefaces;
using ResApi.DTA.Services.Shared;
using ResApi.DTO;
using ResApi.Models;

namespace ResApi.DTA.Services
{
	public class EmployeeService : BaseService<Employee>, IEmployee
	{
        private readonly ILogger<EmployeeService> _logger;
        private readonly IEmployee _emp;
        private readonly DataContext _context;
        public EmployeeService(DataContext context, ILogger<EmployeeService> logger) : base(context)
		{
            _logger = logger;
            _context = context;
        }


        #region PubliMethods

        public async Task<DataResponse<EmployeeDTO>> Authenticate(EmployeeDTO model)
        {
            var response = new DataResponse<EmployeeDTO>() { Data = new EmployeeDTO() };

            #region validation
            if (model == null)
            {
                _logger.LogError("Authenticate: Empty input data");
                response.ResponseCode = EDataResponseCode.NoDataFound;
                response.ErrorMessage = "Te dhenat hyrese jane bosh";
                return response;
            }
            #endregion

            var result = await _context.Employees
                   .AnyAsync(x => x.Username == model.Username && x.Password == model.Password/* && x.Status*/);

            // return null if user not found
            if (result)
            {
                var extEmployeeResult = await _context.Employees
                   .AnyAsync(x => x.Username == model.Username && x.Status == true);

                if (extEmployeeResult)
                {
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
                _logger.LogError("Authenticate: couldn't get customer profile ");
                response.Succeeded = false;
                response.Data = null;
                response.ErrorMessage = "Perdoruesi nuk eshte gjetur!";
                response.ResponseCode = EDataResponseCode.NoDataFound;
                return response;
            }
            

        }

        public async Task<DataResponse<string>> Register(Employee model)
        {
            var response = new DataResponse<string>() { Succeeded = false, Data = string.Empty };

            bool checkIfUserExists = await _context.Employees
                       .AnyAsync(x => x.Username == model.Username); 
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
                employee.RoleId = model.RoleId;
                //employee.Role = model.Role;
                employee.Username = model.Username;
                employee.Password = model.Password;
                //employee.ContactInfo = model.Contact;
                //employee.Status = model.Status;

                //var entity = _emp.Add(employee);
                _emp.Add(employee);
                // Adding the user to context of users.
                if (employee != null)
                {
                    response.Succeeded = true;
                    response.Data = "Error";
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

        public async Task<DataResponse<Employee>> UpdateEmployee(Employee model)
        {
            var response = new DataResponse<Employee>
            {
                Succeeded = false,
                ErrorMessage = "Per shkak te arsyeve teknike nuk mund te perditesojme klientin"
            };

            try
            {
                _emp.Update(model);
                //var entity = _repository.Update(employee); // Update the user to context of users.
                //if (entity != null)
                //{
                //    response.Succeeded = true;
                //    response.Data = true;
                //}
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"UpdateEmployee: Update customer failed when trying to save in Portal db {e.Message}");
                response.ErrorMessage = "Per shkak te problemeve teknike nuk mund te perditesojme profilin";
            }

            return response;
        }

        public async Task<DataResponse<bool>> ChangePassword(ChangePasswordDto model, CancellationToken cancellationToken)
        {
            var response = new DataResponse<bool>
            {
                Succeeded = false,
                ErrorMessage = "Per shkak te arsyeve teknike nuk mund te perditesojme klientin"
            };

            var user = await _emp.Get(model.Id, cancellationToken);

            if (model.newPassword == user.Password)
            {
                if (model.newPassword == model.confirmNewPassword)
                {
                    var resultChange = await _emp.UpdateEmployee(user);
                    response.Succeeded = true;
                    return response;
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

        public async Task<DataResponse<bool>> DisableEmployee(DisableEmployeeDTO model, CancellationToken cancellationToken)
        {
            var response = new DataResponse<bool>
            {
                Succeeded = false,
                ErrorMessage = "Per shkak te arsyeve teknike nuk mund te perditesojme klientin"
            };
            var user = await _emp.Get(model.Id, cancellationToken);
            
                if (user != null)
                {
                    var resultChange = await _emp.UpdateEmployee(user);
                    response.Succeeded = true;
                    return response;
                }
                else
                {
                    _logger.LogError("ChangePassword: Inputs for confirm and new don't match");
                    response.ErrorMessage = "Fjalekalimet e futura nuk perputhen";
                }

            return response;
        }

        //public async Task<DataResponse<AuthenticateUserDto>> Login(string username)
        //{
        //    var response = new DataResponse<AuthenticateUserDto>() { Data = new AuthenticateUserDto() };

        //    #region validation
        //    if (string.IsNullOrEmpty(username))
        //    {
        //        _logger.LogError("Authenticate: Empty input data");
        //        response.ResponseCode = EDataResponseCode.InvalidInputParameter;
        //        response.ErrorMessage = "Te dhenat hyrese jane bosh";
        //        return response;
        //    }
        //    #endregion

        //    var result = await _repository.Login(username);

        //    // return null if user not found
        //    if (result.Succeeded)
        //    {
        //        var extEmployeeResult = GetEmployeeProfile(result.Data.EmployeeID);

        //        if (extEmployeeResult.Succeeded && extEmployeeResult.Data != null)
        //        {
        //            //check is user active in einsure
        //            if (extEmployeeResult.Data.Status == true)
        //            {
        //                var token = GenerateJwtToken(result.Data);
        //                //Set menus for user
        //                response.Data = new AuthenticateUserDto(result.Data.EmployeeID, result.Data.Name, result.Data.Surname, result.Data.RoleID,
        //                                             result.Data.Role, result.Data.Username, result.Data.Password,
        //                                             result.Data.ContactInfo, result.Data.Orders, result.Data.AssignedTables,
        //                                             result.Data.Status, token);
        //                response.Succeeded = true;
        //                return response;
        //            }
        //            else
        //            {
        //                _logger.LogError("Login: User is not activated in eInsure");
        //                response.Succeeded = false;
        //                response.Data = null;
        //                response.ErrorMessage = "Perdoruesi nuk eshte aktiv. Hyrja eshte e ndaluar";// "User is not active, login is forbidden!";
        //                return response;
        //            }
        //        }
        //        else
        //        {
        //            _logger.LogError("Login: couldn't get customer profile from eInsure");
        //            response.Succeeded = false;
        //            response.Data = null;
        //            response.ErrorMessage = extEmployeeResult.ErrorMessage;
        //            response.ResponseCode = EDataResponseCode.NoDataFound;
        //            return response;
        //        }
        //    }

        //    // authentication successful so generate jwt token
        //    response.ErrorMessage = result.ErrorMessage;
        //    response.Succeeded = result.Succeeded;
        //    response.ResponseCode = result.ResponseCode;
        //    return response;
        //}

        #endregion
    }
}

