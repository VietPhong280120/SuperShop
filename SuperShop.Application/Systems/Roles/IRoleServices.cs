using SuperShop.ViewModels.Systems.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperShop.Application.Systems.Roles
{
    public interface IRoleServices
    {
        Task<List<RoleVm>> GetAll();
    }
}