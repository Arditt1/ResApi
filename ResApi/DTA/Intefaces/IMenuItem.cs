
﻿using ResApi.DataResponse;
﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ResApi.DataResponse;
using ResApi.DTA.Intefaces.Shared;
using ResApi.DTO;
using ResApi.Models;
using System.Threading.Tasks;

namespace ResApi.DTA.Intefaces
{
    public interface IMenuItem : IBaseService<MenuItem>
    {
        Task<DataResponse<string>> CreateMenuItem(MenuItemDTO model);
        Task<DataResponse<string>> UpdateMenuItem(MenuItemDTO model);
        Task<List<MenuItemDTO>> GetAllMenuItems(CancellationToken cancellationToken);
        Task<List<MenuItemDTO>> GetAllMenuItemsByCategory(int CategoryId);
    }
}

