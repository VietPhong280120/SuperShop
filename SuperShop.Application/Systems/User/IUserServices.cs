using SuperShop.ViewModels.Common;
using SuperShop.ViewModels.Systems.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperShop.Application.Systems.User
{
    public interface IUserServices
    {
        Task<ApiResult<string>> Authenticate(LoginRequest request);

        Task<ApiResult<bool>> Register(RegisterRequest request);

        Task<ApiResult<bool>> UpdateUser(Guid id, UpdateUserRequest request);

        Task<ApiResult<bool>> DeleteUser(Guid id);

        Task<ApiResult<UserVm>> GetById(Guid id);

        Task<ApiResult<PageResult<UserVm>>> GetUserPaging(GetUserRequest request);

        Task<ApiResult<bool>> RoleAsign(Guid id, RoleAsignRequest request);
    }
}