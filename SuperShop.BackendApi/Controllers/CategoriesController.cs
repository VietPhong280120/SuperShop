using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SuperShop.Application.Catalog.Categories;
using SuperShop.ViewModels.Catalog.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperShop.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryServices _categorySevice;

        public CategoriesController(ICategoryServices categorySevice)

        {
            _categorySevice = categorySevice;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery] string languageId)
        {
            var category = await _categorySevice.GetAll(languageId);
            return Ok(category);
        }

        [HttpGet("{categoryId}/{languageId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int categoryId, string languageId)
        {
            var category = await _categorySevice.GetById(categoryId, languageId);
            return Ok(category);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateCategory([FromForm] CategoryCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var categoryId = await _categorySevice.CreateCategory(request);
            if (categoryId == 0)
            {
                return BadRequest();
            }
            var category = await _categorySevice.GetById(categoryId, request.LanguageId);
            return CreatedAtAction(nameof(GetById), new { Id = categoryId }, category);
        }

        [HttpPost("{categoryId}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateCategory([FromRoute] int categoryId, [FromForm] CategoryUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            request.Id = categoryId;
            var category = await _categorySevice.UpdateCategory(request);
            if (category == 0)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpPut("{categoryId}")]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            var category = await _categorySevice.DeleteCategory(categoryId);
            if (category == 0)
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}