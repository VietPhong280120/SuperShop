using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SuperShop.ViewModels.Catalog.Categories;
using SuperShop.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperShop.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryApiClient _categoryApiClient;

        public CategoryController(ICategoryApiClient categoryApiClient)
        {
            _categoryApiClient = categoryApiClient;
        }

        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 1)
        {
            var languageId = HttpContext.Session.GetString("DefaultLanguage");
            var request = new GetCategoryRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
                LanguageId = languageId,
            };
            var data = await _categoryApiClient.GetPaging(request);
            ViewBag.Keyword = keyword;
            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }
            return View(data);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] CategoryCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }
            var result = await _categoryApiClient.CreateCategory(request);
            if (result)
            {
                TempData["result"] = "Create is successful !";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Create is unsuccessful !");
            return View(request);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            return View(new CategoryDeleteRequest()
            {
                Id = id
            });
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromForm] CategoryDeleteRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }
            var result = await _categoryApiClient.DeleteCategory(request.Id);
            if (result)
            {
                TempData["result"] = "Delete is successful !";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Delete is unsuccessful !");
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var languageId = HttpContext.Session.GetString("DefaultLanguage");
            var request = await _categoryApiClient.GetById(id, languageId);
            var category = new CategoryUpdateRequest
            {
                Id = request.Id,
                Name = request.Name,
                SeoAlias = request.SeoAlias,
                SeoTitle = request.SeoTitle,
                SeoDescription = request.SeoDescription,
            };
            return View(category);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Edit([FromForm] CategoryUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }
            var result = await _categoryApiClient.UpdateCategory(request);
            if (result)
            {
                TempData["result"] = "Update is successful !";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Update is unsuccessful !");
            return View(request);
        }
    }
}