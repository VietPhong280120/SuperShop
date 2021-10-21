using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SuperShop.ViewModels.Catalog.Categories;
using SuperShop.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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

        public async Task<bool> CreateCategory(CategoryCreateRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            var session = _httpContextAccessor.HttpContext.Session.GetString("Tokens");
            var languageid = _httpContextAccessor.HttpContext.Session.GetString("DefaultLanguage");
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            var requestcontent = new MultipartFormDataContent();
            requestcontent.Add(new StringContent(request.Name.ToString()), "name");
            requestcontent.Add(new StringContent(request.SeoDescription.ToString()), "seoDescription");
            requestcontent.Add(new StringContent(request.SeoAlias.ToString()), "seoAlias");
            requestcontent.Add(new StringContent(request.SeoTitle.ToString()), "seoTitle");
            requestcontent.Add(new StringContent(languageid), "languageid");
            var response = await client.PostAsync($"/api/categories/", requestcontent);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateCategory(CategoryUpdateRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            var session = _httpContextAccessor.HttpContext.Session.GetString("Tokens");
            var languageid = _httpContextAccessor.HttpContext.Session.GetString("DefaultLanguage");
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            var requestcontent = new MultipartFormDataContent();
            requestcontent.Add(new StringContent(request.Name.ToString()), "Name");
            requestcontent.Add(new StringContent(request.SeoAlias.ToString()), "SeoAlias");
            requestcontent.Add(new StringContent(request.SeoDescription.ToString()), "SeoDescription");
            requestcontent.Add(new StringContent(request.SeoTitle.ToString()), "SeoTitle");
            requestcontent.Add(new StringContent(languageid), "languageid");
            var response = await client.PutAsync($"/api/categories/{request.Id}", requestcontent);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteCategory(int id)
        {
            var data = await DeleteAsync($"/api/categories/{id}");
            return data;
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

        public async Task<PageResult<CategoryVm>> GetPaging(GetCategoryRequest request)
        {
            var data = await GetAsync<PageResult<CategoryVm>>(
                  $"/api/categories/paging?pageIndex={request.PageIndex}" +
                  $"&pageSize={request.PageSize}" +
                  $"&keyword={request.Keyword}&languageId={request.LanguageId}");

            return data;
        }
    }
}