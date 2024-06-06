using ResApi.Models;
using ResApi.Services.Interface;
using ResApi.DataResponse;
using ResApi.Dtos;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using ResApi.Services.Implementation;
using System.Data.Entity;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity.Infrastructure;

namespace ResApi.Services
{
    public class UserRepository : Repository<DefOperator>, IUserRepository
    {
        private readonly ILogger<DefOperator> logger;

        public UserRepository(IDbContextFactory dbContextFactory, ILogger<DefOperator> logger) : base(dbContextFactory, logger)
        {
            this.logger = logger;
        }

        public async Task<DataResponse<DefOperator>> Authenticate(AuthenticateUserDto authenticateRequest, bool isEmailConfirmationMode)
        {
            var result = new DataResponse<DefOperator> { Data = null, Succeeded = false };

            try
            {
                var entity = await DbContext.DefOperator
                   .FirstOrDefaultAsync(x => x.Email == authenticateRequest.Username);

                if (entity == null)
                {
                    result.ResponseCode = EDataResponseCode.NoDataFound;
                    result.ErrorMessage = "Emri I perdoruesit ose fjalekalimi qe keni future eshte I pavlefshem";// "You have entered an invalid username or password";
                    return result;
                }

                if (isEmailConfirmationMode && !entity.ConfirmedMail)
                {
                    result.ResponseCode = EDataResponseCode.ProfileNotActive;
                    result.ErrorMessage = "Profili nuk është aktiv";// "Profile is not active";
                    return result;
                }

                if (!VerifyPassword(authenticateRequest.Password, entity.PasswordHash, entity.PasswordSalt))
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

        public async Task<DataResponse<bool>> ChangePassword(string email, string newPassword)
        {
            var result = new DataResponse<bool> { Data = false, Succeeded = false };

            try
            {
                var entity = await DbContext.DefOperator
                   .FirstOrDefaultAsync(x => x.Email == email);

                if (entity == null)
                {
                    result.ResponseCode = EDataResponseCode.NoDataFound;
                    result.ErrorMessage = "Account does not exist";
                    return result;
                }

                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(newPassword, out passwordHash, out passwordSalt);

                entity.PasswordHash = passwordHash;
                entity.PasswordSalt = passwordSalt;

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

        public bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)); // Create hash using password salt.
                for (int i = 0; i < computedHash.Length; i++)
                { // Loop through the byte array
                    if (computedHash[i] != passwordHash[i]) return false; // if mismatch
                }
            }
            return true; //if no mismatches.
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> CheckUserExists(string email)
        {
            return await DbContext.DefOperator
                   .AnyAsync(x => x.Email == email);
        }

        public async Task<DefOperator> GetByEmail(string email)
        {
            return await DbContext.DefOperator
                .FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<DefOperator> GetByCodeAndEmail(string verificationCode, string email)
        {
            return await DbContext.DefOperator
                .FirstOrDefaultAsync(x => x.Email == email
                                          && !string.IsNullOrEmpty(x.VerificationCode)
                                          && x.VerificationCode == verificationCode
                                          && !x.ConfirmedMail);
        }

        public async Task<DataResponse<DefOperator>> Login(string email)
        {
            var result = new DataResponse<DefOperator> { Data = null, Succeeded = false };

            try
            {
                var entity = await DbContext.DefOperator
                   .FirstOrDefaultAsync(x => x.Email == email && x.ConfirmedMail);

                if (entity == null)
                {
                    result.ResponseCode = EDataResponseCode.NoDataFound;
                    result.ErrorMessage = "Emri I perdoruesit ose fjalekalimi qe keni future eshte I pavlefshem";// "You have entered an invalid username or password";
                    return result;
                }

                if (!entity.ConfirmedMail)
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

        public Task<bool> DeleteNotConfirmedProfiles(int emailConfirmExpirationMinutes)
        {
            try
            {
                DbContext.Database.ExecuteSqlRaw("dbo.DeleteNotConfirmedProfiles @p0", emailConfirmExpirationMinutes);
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
