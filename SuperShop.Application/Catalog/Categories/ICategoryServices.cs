using SuperShop.ViewModels.Catalog.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperShop.Application.Catalog.Categories
{
    public interface ICategoryServices
    {
        Task<List<CategoryVm>> GetAll(string languageId);

        Task<CategoryVm> GetById(string languageId, int categoryId);

        Task<int> CreateCategory(CategoryCreateRequest request);

        Task<int> DeleteCategory(int id);

        Task<int> UpdateCategory(CategoryUpdateRequest request);
    }
}