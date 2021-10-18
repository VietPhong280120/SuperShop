using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SuperShop.ViewModels.Common;
using SuperShop.ViewModels.Systems.Roles;
using SuperShop.ViewModels.Systems.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SuperShop.Web.Services
{
    public class UserApiClient : BaseApiClient, IUserApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserApiClient(IHttpClientFactory httpClientFactory,
            IConfiguration configuration, IHttpContextAccessor httpContextAccessor) :
            base(httpClientFactory, configuration, httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResult<string>> Authenticate(LoginRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            var response = await client.PostAsync("/api/users/authenticate", httpContent);
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<string>>(await response.Content.ReadAsStringAsync());
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<string>>(await response.Content.ReadAsStringAsync());
        }

        public async Task<bool> DeleteUser(Guid id)
        {
            var data = await DeleteAsync($"/api/users/{id}");
            return data;
        }

        public async Task<ApiResult<UserVm>> GetById(Guid id)
        {
            var data = await GetAsync<ApiResult<UserVm>>($"/api/users/{id}");
            return data;
        }

        public async Task<ApiResult<PageResult<UserVm>>> GetUserPaging(GetUserRequest request)
        {
            var data = await GetAsync<ApiResult<PageResult<UserVm>>>($"/api/users/paging?pageIndex=" +
                $"{request.PageIndex}&pageSize={request.PageSize}&keyword={request.KeyWord}");
            return data;
        }

        public async Task<ApiResult<bool>> RegisterUser(RegisterRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            var json = JsonConvert.SerializeObject(request);
            var httpContext = new StringContent(json, Encoding.UTF8, "application/json");
            var respone = await client.PostAsync("/api/users/register", httpContext);
            var result = await respone.Content.ReadAsStringAsync();
            if (!respone.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
            }
            return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);
        }

        public async Task<ApiResult<bool>> UpdateUser(Guid id, UpdateUserRequest request)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Tokens");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
            var json = JsonConvert.SerializeObject(request);
            var httpContext = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"/api/users/{id}", httpContext);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }

        public async Task<ApiResult<bool>> RoleAssign(Guid id, RoleAsignRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            var session = _httpContextAccessor.HttpContext.Session.GetString("Tokens");
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"/api/users/{id}/roles", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }

        public async Task<ApiResult<List<RoleVm>>> GetRole()
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Tokens");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
            var response = await client.GetAsync("/api/users/roles");
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                List<RoleVm> myDeserializedObjList = (List<RoleVm>)JsonConvert.DeserializeObject(body, typeof(List<RoleVm>));
                return new ApiSuccessResult<List<RoleVm>>(myDeserializedObjList);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<List<RoleVm>>>(body);
        }
    }
}