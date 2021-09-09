using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SuperShop.Application.Common;
using SuperShop.Data.EF;
using SuperShop.Data.Entities;
using SuperShop.ViewModels.Catalog.Common;
using SuperShop.ViewModels.Catalog.Products;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SuperShop.Application.Catalog.Products
{
    public class ProductServices : IProductServices
    {
        private readonly SuperShopDbContext _context;
        private readonly IStorageServices _storageServices;
        private readonly string _USER_CONTENT_FOLDER = "user-content";

        public ProductServices(SuperShopDbContext context, IStorageServices storageServices)
        {
            _context = context;
            _storageServices = storageServices;
        }

        public async Task<int> AddImage(int productId, ProductImageCreateRequest request)
        {
            var productImage = new ProductImage()
            {
                Caption = request.Caption,
                SortOrder = request.SortOrder,
                DateCreated = DateTime.Now,
                ProductId = productId,
                IsDefault = request.IsDefault
            };
            if (request.ImageFile != null)
            {
                productImage.ImagePath = await this.SaveFile(request.ImageFile);
                productImage.Filesize = request.ImageFile.Length;
            }
            _context.ProductImages.Add(productImage);
            await _context.SaveChangesAsync();
            return productImage.Id;
        }

        public async Task<int> CreateProduct(ProductCreateRequest request)
        {
            var languages = _context.Languages;
            var tranlations = new List<ProductTranslation>();
            foreach (var language in languages)
            {
                if (language.Id == request.LanguageId)
                {
                    tranlations.Add(new ProductTranslation()
                    {
                        Name = request.Name,
                        Description = request.Description,
                        Detail = request.Detail,
                        SeoDescription = request.SeoDescription,
                        SeoAlias = request.SeoAlias,
                        SeoTitle = request.SeoTitle,
                        LanguageId = request.LanguageId
                    });
                }
                else
                {
                    tranlations.Add(new ProductTranslation()
                    {
                        Name = "N/A",
                        Description = "N/A",
                        Detail = "N/A",
                        SeoAlias = "N/A",
                        SeoDescription = "N/A",
                        SeoTitle = "N/A",
                        LanguageId = language.Id
                    });
                }
            }
            var product = new Product()
            {
                Price = request.Price,
                OriginalPrice = request.OriginalPrice,
                Stock = request.Stock,
                DateCreate = DateTime.Now,
                SeoAlias = request.SeoAlias,
                ProductTranslations = tranlations
            };
            if (request.ThumbnailImage != null)
            {
                product.ProductImages = new List<ProductImage>()
                {
                    new ProductImage()
                    {
                        Caption = "Thumbnail Image",
                        DateCreated = DateTime.Now,
                        Filesize = request.ThumbnailImage.Length,
                        ImagePath = await this.SaveFile(request.ThumbnailImage),
                        IsDefault=true,
                        SortOrder=1
                    }
                };
            }
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product.Id;
        }

        public async Task<int> DeleteProduct(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            var images = _context.ProductImages.Where(i => i.ProductId == productId);
            foreach (var image in images)
            {
                await _storageServices.DeleteFileAsync(image.ImagePath);
            }
            _context.Products.Remove(product);
            return await _context.SaveChangesAsync();
        }

        public async Task<PageResult<ProductVm>> GetAllPaging(GetProductPagingRequest request)
        {
            //1 Select with leftjoin
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pc in _context.ProductInCategories on p.Id equals pc.ProductId into ppc
                        from pc in ppc.DefaultIfEmpty()
                        join c in _context.Categories on pc.CategoryId equals c.Id into pic
                        from c in pic.DefaultIfEmpty()
                        join pi in _context.ProductImages on p.Id equals pi.ProductId into ppi
                        from pi in ppi.DefaultIfEmpty()
                        where pt.LanguageId == request.LanguageId && pi.IsDefault == true
                        select new { p, pt, pc, pi };
            //2 filter
            if (!string.IsNullOrEmpty(request.Key))
            {
                query = query.Where(x => x.pt.Name.Contains(request.Key));
            }
            if (request.CategoryId != null && request.CategoryId != 0)
            {
                query = query.Where(p => p.pc.CategoryId == request.CategoryId);
            }
            //3 Paging
            int totalRow = await query.CountAsync();
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new ProductVm()
                {
                    Id = x.p.Id,
                    Name = x.pt.Name,
                    DateCreated = x.p.DateCreate,
                    Description = x.pt.Description,
                    Detail = x.pt.Detail,
                    LanguageId = x.pt.LanguageId,
                    OriginalPrice = x.p.OriginalPrice,
                    Price = x.p.Price,
                    SeoAlias = x.pt.SeoAlias,
                    SeoDescription = x.pt.SeoDescription,
                    SeoTitle = x.pt.SeoTitle,
                    Stock = x.p.Stock,
                    ThumbnailImage = x.pi.ImagePath
                }).ToListAsync();
            //4 select and project
            var pageResult = new PageResult<ProductVm>()
            {
                TotalRecord = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };
            return pageResult;
        }

        public async Task<PageResult<ProductVm>> GetByCategoryId(string languageId, GetProductPagingRequest request)
        {
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pc in _context.ProductInCategories on p.Id equals pc.ProductId
                        join c in _context.Categories on pc.CategoryId equals c.Id
                        where pt.LanguageId == languageId
                        select new { p, pt, pc };
            //2 filter
            if (request.CategoryId.HasValue && request.CategoryId.Value > 0)
            {
                query = query.Where(p => p.pc.CategoryId == request.CategoryId);
            }
            //3 Paging
            int totalRow = await query.CountAsync();
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new ProductVm()
                {
                    Id = x.p.Id,
                    Name = x.pt.Name,
                    DateCreated = x.p.DateCreate,
                    Description = x.pt.Description,
                    Detail = x.pt.Detail,
                    LanguageId = x.pt.LanguageId,
                    OriginalPrice = x.p.OriginalPrice,
                    Price = x.p.Price,
                    SeoAlias = x.pt.SeoAlias,
                    SeoDescription = x.pt.SeoDescription,
                    SeoTitle = x.pt.SeoTitle,
                    Stock = x.p.Stock,
                }).ToListAsync();
            //4 select and project
            var pageResult = new PageResult<ProductVm>()
            {
                TotalRecord = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };
            return pageResult;
        }

        public async Task<ProductVm> GetById(int productId, string languageId)
        {
            var product = await _context.Products.FindAsync();
            var productTranslation = await _context.ProductTranslations.FirstOrDefaultAsync(x => x.ProductId == productId
            && x.LanguageId == languageId);
            var categories = await (from c in _context.Categories
                                    join ct in _context.CategoryTranslations on c.Id equals ct.CategoryId
                                    join pic in _context.ProductInCategories on c.Id equals pic.CategoryId
                                    where pic.ProductId == productId && ct.LanguageId == languageId
                                    select ct.Name
                                    ).ToListAsync();
            var image = await _context.ProductImages.Where(x => x.ProductId == productId && x.IsDefault == true).FirstOrDefaultAsync();
            var productVm = new ProductVm()
            {
                Id = product.Id,
                DateCreated = product.DateCreate,
                LanguageId = productTranslation.LanguageId,
                Price = product.Price,
                OriginalPrice = product.OriginalPrice,
                Stock = product.Stock,
                Description = productTranslation != null ? productTranslation.Description : null,
                SeoAlias = productTranslation != null ? productTranslation.SeoAlias : null,
                Detail = productTranslation != null ? productTranslation.Detail : null,
                SeoDescription = productTranslation != null ? productTranslation.SeoDescription : null,
                SeoTitle = productTranslation != null ? productTranslation.SeoTitle : null,
                Categories = categories,
                ThumbnailImage = image != null ? image.ImagePath : "no-image.jpg"
            };
            return productVm;
        }

        public async Task<int> RemoveImage(int imageId)
        {
            var productImage = await _context.ProductImages.FindAsync(imageId);
            _context.ProductImages.Remove(productImage);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateImage(int imageId, ProductImageUpdateRequest request)
        {
            var productImage = await _context.ProductImages.FindAsync(imageId);
            if (request.ThumbnailImage != null)
            {
                productImage.ImagePath = await this.SaveFile(request.ThumbnailImage);
                productImage.Filesize = request.ThumbnailImage.Length;
            }
            _context.ProductImages.Add(productImage);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateProduct(ProductUpdateRequest request)
        {
            var product = await _context.Products.FindAsync(request.Id);
            var productTranslation = await _context.ProductTranslations.FirstOrDefaultAsync
                (x => x.ProductId == request.Id && x.LanguageId == request.LanguageId);
            productTranslation.Name = request.Name;
            productTranslation.SeoAlias = request.SeoAlias;
            productTranslation.SeoDescription = request.SeoDescription;
            productTranslation.SeoTitle = request.SeoTitle;
            productTranslation.Detail = request.Detail;
            product.Price = request.Price;
            product.OriginalPrice = request.OriginalPrice;
            product.Stock = request.Stock;
            if (request.ThumbnailImage != null)
            {
                var thumbnailImage = await _context.ProductImages.FirstOrDefaultAsync
                    (x => x.IsDefault == true && x.ProductId == request.Id);
                if (thumbnailImage != null)
                {
                    thumbnailImage.Filesize = request.ThumbnailImage.Length;
                    thumbnailImage.ImagePath = await this.SaveFile(request.ThumbnailImage);
                    _context.ProductImages.Update(thumbnailImage);
                }
            }
            return await _context.SaveChangesAsync();
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFile = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFile)}";
            await _storageServices.SaveFileAsync(file.OpenReadStream(), fileName);
            return "/" + _USER_CONTENT_FOLDER + "/" + fileName;
        }
    }
}