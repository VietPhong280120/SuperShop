using Microsoft.EntityFrameworkCore;
using SuperShop.Data.EF;
using SuperShop.Data.Entities;
using SuperShop.ViewModels.Catalog.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperShop.Application.Catalog.Categories
{
    public class CategoryServices : ICategoryServices
    {
        private readonly SuperShopDbContext _context;

        public CategoryServices(SuperShopDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateCategory(CategoryCreateRequest request)
        {
            var languages = _context.Languages;
            var tranlations = new List<CategoryTranslation>();
            foreach (var language in languages)
            {
                if (language.Id == request.LanguageId)
                {
                    tranlations.Add(new CategoryTranslation()
                    {
                        Name = request.Name,
                        SeoAlias = request.SeoAlias,
                        SeoDescription = request.SeoDescription,
                        SeoTitle = request.SeoTitle,
                        LanguageId = request.LanguageId
                    });
                }
                else
                {
                    tranlations.Add(new CategoryTranslation()
                    {
                        Name = "N/A",
                        SeoTitle = "N/A",
                        SeoDescription = "N/A",
                        SeoAlias = "N/A",
                        LanguageId = language.Id
                    });
                }
            }
            var categories = new Category()
            {
                CategoryTranslations = tranlations
            };
            _context.Add(categories);
            await _context.SaveChangesAsync();
            return categories.Id;
        }

        public async Task<int> DeleteCategory(int categoryId)
        {
            var categories = await _context.Categories.FindAsync(categoryId);
            _context.Categories.Remove(categories);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<CategoryVm>> GetAll(string languageId)
        {
            var query = from c in _context.Categories
                        join ct in _context.CategoryTranslations on c.Id equals ct.CategoryId
                        where ct.LanguageId == languageId
                        select new { c, ct };
            var data = await query.Select(x => new CategoryVm()
            {
                Id = x.c.Id,
                Name = x.ct.Name,
                ParentId = x.c.ParentId
            }).ToListAsync();
            return data;
        }

        public async Task<CategoryVm> GetById(string languageId, int id)
        {
            var query = from c in _context.Categories
                        join ct in _context.CategoryTranslations on c.Id equals ct.CategoryId
                        where ct.LanguageId == languageId
                        select new { c, ct };
            var data = await query.Select(x => new CategoryVm()
            {
                Id = x.c.Id,
                Name = x.ct.Name,
                ParentId = x.c.ParentId
            }).FirstOrDefaultAsync();
            return data;
        }

        public async Task<int> UpdateCategory(CategoryUpdateRequest request)
        {
            var categories = await _context.Categories.FindAsync(request.Id);
            var tranlation = await _context.CategoryTranslations.FirstOrDefaultAsync
                (x => x.CategoryId == request.Id && x.LanguageId == request.LanguageId);
            tranlation.Name = request.Name;
            tranlation.SeoAlias = request.SeoAlias;
            tranlation.SeoDescription = request.SeoDescription;
            tranlation.SeoTitle = request.SeoTitle;
            return await _context.SaveChangesAsync();
        }
    }
}