using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SuperShop.ViewModels.Common;
using SuperShop.ViewModels.Catalog.Products;
using SuperShop.ViewModels.Catalog.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SuperShop.Web.Services
{
    public interface IProductApiClient
    {
        Task<PageResult<ProductVm>> GetPaging(GetProductPagingRequest request);

        Task<bool> CreateProduct(ProductCreateRequest request);

        Task<bool> UpdateProduct(ProductUpdateRequest request);

        Task<bool> DeleteProduct(int id);

        Task<ProductVm> GetById(int id, string languageId);

        Task<ApiResult<bool>> CategoryAssign(int id, CategoryAssignRequest request);
    }
}