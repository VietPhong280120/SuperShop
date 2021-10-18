using SuperShop.ViewModels.Common;
using SuperShop.ViewModels.Systems.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperShop.Application.Systems.Language
{
    public interface ILanguageServices
    {
        Task<ApiResult<List<LanguageVm>>> GetAll();
    }
}