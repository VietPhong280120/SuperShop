using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SuperShop.Application.Systems.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperShop.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LanguagesController : ControllerBase
    {
        private readonly ILanguageServices _languageServices;

        public LanguagesController(ILanguageServices languageServices)
        {
            _languageServices = languageServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var languages = await _languageServices.GetAll();
            return Ok(languages);
        }
    }
}