using SuperShop.ViewModels.Catalog.Categories;
using SuperShop.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperShop.Web.Services
{
    public interface ICategoryApiClient
    {
        Task<List<CategoryVm>> GetAll(string languageid);

        Task<CategoryVm> GetById(int id, string languageid);

        Task<PageResult<CategoryVm>> GetPaging(GetCategoryRequest request);

        Task<bool> CreateCategory(CategoryCreateRequest request);

        Task<bool> DeleteCategory(int id);

        Task<bool> UpdateCategory(CategoryUpdateRequest request);
    }
}