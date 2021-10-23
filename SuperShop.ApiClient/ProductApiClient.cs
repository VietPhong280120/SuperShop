using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SuperShop.ViewModels.Catalog.Products;
using SuperShop.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SuperShop.Web.Services
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

        public async Task<bool> CreateProduct(ProductCreateRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            var session = _httpContextAccessor.HttpContext.Session.GetString("Tokens");
            var languageid = _httpContextAccessor.HttpContext.Session.GetString("DefaultLanguage");
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            var requestContent = new MultipartFormDataContent();

            if (request.ThumbnailImage != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.ThumbnailImage.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.ThumbnailImage.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "thumbnailImage", request.ThumbnailImage.FileName);
            }
            requestContent.Add(new StringContent(request.Name.ToString()), "name");
            requestContent.Add(new StringContent(request.Price.ToString()), "price");
            requestContent.Add(new StringContent(request.OriginalPrice.ToString()), "originalprice");
            requestContent.Add(new StringContent(request.Stock.ToString()), "stock");
            requestContent.Add(new StringContent(request.Description.ToString()), "description");
            requestContent.Add(new StringContent(request.Detail.ToString()), "detail");
            requestContent.Add(new StringContent(request.SeoDescription.ToString()), "seodescription");
            requestContent.Add(new StringContent(request.SeoAlias.ToString()), "seoalias");
            requestContent.Add(new StringContent(request.SeoTitle.ToString()), "seotitle");
            requestContent.Add(new StringContent(languageid), "languageid");
            var respone = await client.PostAsync($"/api/products/", requestContent);
            return respone.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteProduct(int id)
        {
            var data = await DeleteAsync($"api/products/{id}");
            return data;
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

        public async Task<bool> UpdateProduct(ProductUpdateRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            var session = _httpContextAccessor.HttpContext.Session.GetString("Tokens");
            var languageid = _httpContextAccessor.HttpContext.Session.GetString("DefaultLanguage");
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            var requestContent = new MultipartFormDataContent();
            if (request.ThumbnailImage != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.ThumbnailImage.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.ThumbnailImage.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "thumnailImage", request.ThumbnailImage.FileName);
            }
            requestContent.Add(new StringContent(request.Name.ToString()), "name");
            requestContent.Add(new StringContent(request.Price.ToString()), "price");
            requestContent.Add(new StringContent(request.OriginalPrice.ToString()), "originalprice");
            requestContent.Add(new StringContent(request.Stock.ToString()), "stock");
            requestContent.Add(new StringContent(request.Description.ToString()), "description");
            requestContent.Add(new StringContent(request.Detail.ToString()), "detail");
            requestContent.Add(new StringContent(request.SeoDescription.ToString()), "seodescription");
            requestContent.Add(new StringContent(request.SeoAlias.ToString()), "seoalias");
            requestContent.Add(new StringContent(request.SeoTitle.ToString()), "seotitle");
            requestContent.Add(new StringContent(languageid), "languageid");
            var respone = await client.PutAsync($"/api/products/{request.Id}", requestContent);
            return respone.IsSuccessStatusCode;
        }

        public async Task<ApiResult<bool>> CategoryAssign(int id, CategoryAssignRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            var session = _httpContextAccessor.HttpContext.Session.GetString("Tokens");
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            var json = JsonConvert.SerializeObject(request);
            var httpContext = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"/api/products/{id}/categories", httpContext);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }
    }
}