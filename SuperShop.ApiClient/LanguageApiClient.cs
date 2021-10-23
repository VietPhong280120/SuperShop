using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SuperShop.ViewModels.Common;
using SuperShop.ViewModels.Systems.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SuperShop.Web.Services
{
    public class LanguageApiClient : BaseApiClient, ILanguageApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LanguageApiClient(IHttpContextAccessor httpContextAccessor, IHttpClientFactory httpClientFactory,
            IConfiguration configuration) : base(httpClientFactory, configuration, httpContextAccessor)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResult<List<LanguageVm>>> GetAll()
        {
            return await GetAsync<ApiResult<List<LanguageVm>>>("/api/languages");
        }
    }
}