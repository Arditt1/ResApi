using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ResApi.DataResponse;
using ResApi.DTA.Intefaces;
using ResApi.DTA.Services.Shared;
using ResApi.DTO.Permission;
using ResApi.Models;

namespace ResApi.DTA.Services
{
	public class PermissionService : BaseService<Permission>, IPermission
    {
		private readonly ILogger<Permission> _logger;
		private readonly DataContext _context;
		private readonly IMapper _mapper;
		public PermissionService(DataContext context, ILogger<Permission> logger, IMapper mapper) : base(context)
		{
			_logger = logger;
			_context = context;
			_mapper = mapper;
		}

        public async Task<DataResponse<string>> CreatePermission(PermissionDTO model)
        {
            var response = new DataResponse<string>() { Succeeded = false, Data = string.Empty };

            bool checkIfOrderExists = await _context.Permissions
                       .AnyAsync(x => x.Permission1 == model.Permission1);
            if (checkIfOrderExists)
            {
                response.ErrorMessage = "Porosia me id: " + model.Id + " ekziston";
                return response;
            }

            try
            {
                var PerMapp = _mapper.Map<Permission>(model);
                _context.Permissions.Add(PerMapp);
                _context.SaveChanges();
                // Adding the user to context of users.
                if (PerMapp != null)
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
                //RequestLogger.WriteResAPIRequests("HTTP POST Response CreatePermission: ", response);
            }
            return response;
        }

        public async Task<DataResponse<string>> UpdatePermission(PermissionDTO model)
        {
            var response = new DataResponse<string>
            {
                Succeeded = false,
                ErrorMessage = "Per shkak te arsyeve teknike nuk mund te perditesojme klientin"
            };

            try
            {
                var PerMapp = _mapper.Map<Permission>(model);
                _context.Permissions.Update(PerMapp);
                //var entity = _repository.Update(employee); // Update the user to context of users.
                if (PerMapp != null)
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
                //RequestLogger.WriteResAPIRequests("HTTP POST Response UpdatePermission: ", response);
                response.ErrorMessage = "Per shkak te problemeve teknike nuk mund te perditesojme profilin";
            }

            return response;
        }
    }
}

