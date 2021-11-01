using SuperShop.ViewModels.Catalog.Products;
using SuperShop.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperShop.Page.Services
{
    public interface IProductApiClient
    {
        Task<PageResult<ProductVm>> GetPaging(GetProductPagingRequest request);

        Task<ProductVm> GetById(int id, string languageId);
    }
}