using SuperShop.ViewModels.Common;
using SuperShop.ViewModels.Systems.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperShop.Web.Services
{
    public interface ILanguageApiUser
    {
        Task<ApiResult<List<LanguageVm>>> GetAll();
    }
}