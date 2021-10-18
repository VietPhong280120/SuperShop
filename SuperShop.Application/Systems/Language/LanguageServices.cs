using Microsoft.EntityFrameworkCore;
using SuperShop.Data.EF;
using SuperShop.ViewModels.Common;
using SuperShop.ViewModels.Systems.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperShop.Application.Systems.Language
{
    public class LanguageServices : ILanguageServices
    {
        private readonly SuperShopDbContext _context;

        public LanguageServices(SuperShopDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResult<List<LanguageVm>>> GetAll()
        {
            var languages = await _context.Languages.Select(x => new LanguageVm()
            {
                Id = x.Id,
                Name = x.Name
            }).ToListAsync();
            return new ApiSuccessResult<List<LanguageVm>>(languages);
        }
    }
}