using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ResApi.DataResponse;
using ResApi.DTA.Intefaces;
using ResApi.DTA.Services.Shared;
using ResApi.DTO.Role;
using ResApi.Models;

namespace ResApi.DTA.Services
{
	public class RoleService : BaseService<Role>, IRole
    {
        private readonly ILogger<Role> _logger;
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public RoleService(DataContext context, ILogger<Role> logger, IMapper mapper) : base(context)
		{
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        public async Task<DataResponse<string>> CreateRole(RoleDTO model)
        {
            var response = new DataResponse<string>() { Succeeded = false, Data = string.Empty };

            try
            {
                var RoleMapp = _mapper.Map<Role>(model);
                _context.Roles.Add(RoleMapp);
                _context.SaveChanges();
                // Adding the user to context of users.
                if (RoleMapp != null)
                {
                    response.Succeeded = true;
                    response.Data = "Success";
                    return response;
                }
                else
                {
                    response.Succeeded = false;
                    response.Data = "Failure";
                    return response;
                }
            }
            catch (Exception e)
            {
                response.ErrorMessage = "Per shkak te problemeve teknike nuk jemi ne gjendje te krijojme profilin.";
                //RequestLogger.WriteResAPIRequests("HTTP POST Response CreateRole: ", response);
            }
            return response;
        }

        public async Task<DataResponse<string>> UpdateRole(RoleDTO model)
        {
            var response = new DataResponse<string>
            {
                Succeeded = false,
                ErrorMessage = "Per shkak te arsyeve teknike nuk mund te perditesojme klientin"
            };

            try
            {
                var RoleMapp = _mapper.Map<Role>(model);
                _context.Roles.Update(RoleMapp);
                //var entity = _repository.Update(employee); // Update the user to context of users.
                if (RoleMapp != null)
                {
                    response.Succeeded = true;
                    response.Data = "Success";
                }
                else
                {
                    response.Succeeded = false;
                    response.Data = "Error";
                }
            }
            catch (Exception e)
            {
                //RequestLogger.WriteResAPIRequests("HTTP POST Response UpdateRole: ", response);
                response.ErrorMessage = "Per shkak te problemeve teknike nuk mund te perditesojme profilin";
            }

            return response;
        }

        public async Task<List<RoleDTO>> GetEmployeesByRole(string roleName)
        {
            try
            {
                var Roles = await _context.Roles
                    .Include(e => e.Employees) // Assuming Employees have a navigation property to Roles
                    .Where(x => x.RoleName == roleName)
                    .Select(e => new RoleDTO
                    {
                        Id = e.Id
                        // Map other properties as needed
                    })
                    .ToListAsync();

                return Roles;
            }
            catch
            {
                throw;
            }
        }
    }
}

