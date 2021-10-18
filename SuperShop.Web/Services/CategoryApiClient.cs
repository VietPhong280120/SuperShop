using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SuperShop.ViewModels.Catalog.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SuperShop.Web.Services
{
    public class CategoryApiClient : BaseApiClient, ICategoryApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CategoryApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, configuration, httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public async Task<List<CategoryVm>> GetAll(string languageid)
        {
            var data = await GetListAsync<CategoryVm>($"/api/categories?languageid={languageid}");
            return data;
        }

        public async Task<CategoryVm> GetById(int id, string languageid)
        {
            var data = await GetAsync<CategoryVm>($"api/categories/{id}/{languageid}");
            return data;
        }
    }
}