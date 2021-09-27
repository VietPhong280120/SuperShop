using SuperShop.ViewModels.Common;
using SuperShop.ViewModels.Systems.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperShop.Web.Services
{
    public interface IUserApiClient
    {
        public Task<ApiResult<string>> Authenticate(LoginRequest request);

        public Task<ApiResult<PageResult<UserVm>>> GetUserPaging(GetUserRequest request);
    }
}