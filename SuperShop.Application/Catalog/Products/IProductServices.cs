using SuperShop.ViewModels.Common;
using SuperShop.ViewModels.Catalog.Products;
using System.Threading.Tasks;

namespace SuperShop.Application.Catalog.Products
{
    public interface IProductServices
    {
        Task<int> CreateProduct(ProductCreateRequest request);

        Task<int> DeleteProduct(int productId);

        Task<int> UpdateProduct(ProductUpdateRequest request);

        Task<ProductVm> GetById(int productId, string languageId);

        Task<PageResult<ProductVm>> GetAllPaging(GetProductPagingRequest request);

        Task<PageResult<ProductVm>> GetByCategoryId(string languageId, GetProductPagingRequest request);

        Task<int> AddImage(int productId, ProductImageCreateRequest request);

        Task<int> UpdateImage(int imageId, ProductImageUpdateRequest request);

        Task<int> RemoveImage(int imageId);
    }
}