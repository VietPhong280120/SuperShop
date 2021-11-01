using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SuperShop.ViewModels.Catalog.Products;
using SuperShop.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SuperShop.Page.Services
{
    public class ProductApiClient : BaseApiClient, IProductApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductApiClient(IHttpContextAccessor httpContextAccessor, IHttpClientFactory httpClientFactory,
            IConfiguration configuration) : base(httpClientFactory, configuration, httpContextAccessor)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ProductVm> GetById(int id, string languageId)
        {
            var data = await GetAsync<ProductVm>($"/api/products/{id}/{languageId}");
            return data;
        }

        public async Task<PageResult<ProductVm>> GetPaging(GetProductPagingRequest request)
        {
            var data = await GetAsync<PageResult<ProductVm>>(
                 $"/api/products/paging?pageIndex={request.PageIndex}" +
                 $"&pageSize={request.PageSize}" +
                 $"&keyword={request.Keyword}&languageId={request.LanguageId}&categoryId={request.CategoryId}");

            return data;
        }
    }
}