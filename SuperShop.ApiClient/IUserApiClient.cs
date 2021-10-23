using SuperShop.ViewModels.Common;
using SuperShop.ViewModels.Systems.Roles;
using SuperShop.ViewModels.Systems.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperShop.Web.Services
{
    public interface IUserApiClient
    {
        Task<ApiResult<string>> Authenticate(LoginRequest request);

        Task<ApiResult<PageResult<UserVm>>> GetUserPaging(GetUserRequest request);

        Task<ApiResult<bool>> RegisterUser(RegisterRequest request);

        Task<ApiResult<bool>> UpdateUser(Guid id, UpdateUserRequest request);

        Task<bool> DeleteUser(Guid Id);

        Task<ApiResult<UserVm>> GetById(Guid id);

        Task<ApiResult<List<RoleVm>>> GetRole();

        Task<ApiResult<bool>> RoleAssign(Guid id, RoleAsignRequest request);
    }
}