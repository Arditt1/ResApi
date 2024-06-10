using ResApi.Models;
using ResApi.Services.Interface;
using ResApi.DataResponse;
using ResApi.Dtos;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using ResApi.Services.Implementation;
using Microsoft.EntityFrameworkCore;


namespace ResApi.Services
{
    public class UserRepository : Repository<Employee>, IUserRepository
    {
        private readonly ILogger<Employee> logger;

        public UserRepository(IDbContextFactory dbContextFactory, ILogger<Employee> logger) : base(dbContextFactory, logger)
        {
            this.logger = logger;
        }

        public async Task<DataResponse<Employee>> Authenticate(AuthenticateUserDto authenticateRequest)
        {
            var result = new DataResponse<Employee> { Data = null, Succeeded = false };

            try
            {
                var entity = await DbContext.Employee
                   .FirstOrDefaultAsync(x => x.Username == authenticateRequest.Username);

                if (entity == null)
                {
                    result.ResponseCode = EDataResponseCode.NoDataFound;
                    result.ErrorMessage = "Emri I perdoruesit ose fjalekalimi qe keni future eshte I pavlefshem";// "You have entered an invalid username or password";
                    return result;
                }

                if (!entity.Status)
                {
                    result.ResponseCode = EDataResponseCode.ProfileNotActive;
                    result.ErrorMessage = "Profili nuk është aktiv";// "Profile is not active";
                    return result;
                }

                var entity_pass = await DbContext.Employee
                    .FirstOrDefaultAsync(x => x.Password == entity.Password);
                if (entity_pass == null)
                {
                    result.ResponseCode = EDataResponseCode.NoDataFound;
                    result.ErrorMessage = "Emri I perdoruesit ose fjalekalimi qe keni future eshte I pavlefshem";// "You have entered an invalid username or password";
                    return result;
                }

                result.Data = entity;
                result.Succeeded = true;
                return result;
            }
            catch (Exception ex)
            {
                result.ResponseCode = EDataResponseCode.GenericError;
                result.ErrorMessage = "Aplikacioni ka hasur ne nje gabim te panjohur. Ju lutem kontaktoni stafin tone teknik!.";
                logger.LogError(ex, "Error occurred on authenticating user");
                return result;
            }
        }

        public async Task<DataResponse<bool>> ChangePassword(string username, string newPassword)
        {
            var result = new DataResponse<bool> { Data = false, Succeeded = false };

            try
            {
                var entity = await DbContext.Employee
                   .FirstOrDefaultAsync(x => x.Username == username);

                if (entity == null)
                {
                    result.ResponseCode = EDataResponseCode.NoDataFound;
                    result.ErrorMessage = "Account does not exist";
                    return result;
                }

                entity.Password = newPassword;

                var changedEntity = await this.UpdateEntity(entity);

                if (changedEntity != null)
                {
                    result.Data = true;
                    result.Succeeded = true;
                    return result;
                }
                else
                {
                    result.ResponseCode = EDataResponseCode.GenericError;
                    result.ErrorMessage = "Error occured when trying to update password for user";
                    return result;
                }
            }
            catch (Exception ex)
            {
                result.ResponseCode = EDataResponseCode.GenericError;
                result.ErrorMessage = "Aplikacioni ka hasur ne nje gabim te panjohur. Ju lutem kontaktoni stafin tone teknik!.";
                logger.LogError(ex, "Error occurred on authenticating user");
                return result;
            }
        }

        public async Task<bool> CheckUserExists(string username)
        {
            return await DbContext.Employee
                   .AnyAsync(x => x.Username == username && x.Status);
        }

        //public async Task<Employee> GetByUsername(string username)
        //{
        //    return await DbContext.Employee
        //        .FirstOrDefaultAsync(x => x.Username == username);
        //}

        public async Task<DataResponse<Employee>> Login(string username)
        {
            var result = new DataResponse<Employee> { Data = null, Succeeded = false };

            try
            {
                var entity = await DbContext.Employee
                   .FirstOrDefaultAsync(x => !(x.Username != username));

                if (entity == null)
                {
                    result.ResponseCode = EDataResponseCode.NoDataFound;
                    result.ErrorMessage = "Emri I perdoruesit ose fjalekalimi qe keni future eshte I pavlefshem";// "You have entered an invalid username or password";
                    return result;
                }

                if (!entity.Status)
                {
                    result.ResponseCode = EDataResponseCode.ProfileNotActive;
                    result.ErrorMessage = "Profili nuk është aktiv";// "Profile is not active";
                    return result;
                }

                result.Data = entity;
                result.Succeeded = true;
                return result;
            }
            catch (Exception ex)
            {
                result.ResponseCode = EDataResponseCode.GenericError;
                result.ErrorMessage = "Aplikacioni ka hasur ne nje gabim te panjohur. Ju lutem kontaktoni stafin tone teknik!.";
                logger.LogError(ex, "Error occurred on authenticating user");
                return result;
            }
        }

        public Task<bool> DeleteEmployee(int Username)
        {
            try
            {
                DbContext.Database.ExecuteSqlRaw("dbo.DeleteNotConfirmedProfiles @p0", Username);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred on delete not confirmed profiles");
                //throw;
            }
            return Task.FromResult(true);
        }
    }
}
