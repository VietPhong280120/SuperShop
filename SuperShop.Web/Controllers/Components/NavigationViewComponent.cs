using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SuperShop.Web.Models;
using SuperShop.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperShop.Web.Controllers.Components
{
    public class NavigationViewComponent : ViewComponent
    {
        private readonly ILanguageApiUser _languageApiClient;

        public NavigationViewComponent(ILanguageApiUser languageApiClient)
        {
            _languageApiClient = languageApiClient;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var language = await _languageApiClient.GetAll();
            var navigation = new NavigationViewModel()
            {
                Languages = language.ResultObj,
                CurrentLanguageId = HttpContext.Session.GetString("DefaultLanguage")
            };
            return View("Default", navigation);
        }
    }
}