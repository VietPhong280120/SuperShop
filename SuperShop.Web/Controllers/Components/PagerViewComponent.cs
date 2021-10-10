using Microsoft.AspNetCore.Mvc;
using SuperShop.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperShop.Web.Controllers.Components
{
    public class PagerViewComponent : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(PageResultBase result)
        {
            return Task.FromResult((IViewComponentResult)View("Default", result));
        }
    }
}