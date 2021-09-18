using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SuperShop.Application.Catalog.Products;
using SuperShop.ViewModels.Catalog.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperShop.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductServices _productServices;

        public ProductsController(IProductServices productServices)
        {
            _productServices = productServices;
        }

        [HttpGet("{paging}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetProductPagingRequest request)
        {
            var product = await _productServices.GetAllPaging(request);
            return Ok(product);
        }

        [HttpGet("{productId}/{languageId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int productId, string languageId)
        {
            var product = await _productServices.GetById(productId, languageId);
            return Ok(product);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateProduct([FromForm] ProductCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var productId = await _productServices.CreateProduct(request);
            if (productId == 0)
            {
                return BadRequest();
            }
            var product = await _productServices.GetById(productId, request.LanguageId);
            return CreatedAtAction(nameof(GetById), new { Id = productId }, product);
        }

        [HttpPut("{productId}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateProduct([FromRoute] int productId, [FromForm] ProductUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            request.Id = productId;
            var result = await _productServices.UpdateProduct(request);
            if (result == 0)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            var result = await _productServices.DeleteProduct(productId);
            if (result == 0)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpGet("{productId}/images/{imageId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetImageId(int productId, int imageId)
        {
            var image = await _productServices.GetImageId(productId, imageId);
            if (image == null)
            {
                return BadRequest("cannot find image");
            }
            return Ok(image);
        }

        [HttpPost("{productId}/images")]
        public async Task<IActionResult> AddImage([FromRoute] int productId, [FromForm] ProductImageCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var imageId = await _productServices.AddImage(productId, request);
            if (productId == 0)
            {
                return BadRequest();
            }
            var image = await _productServices.GetImageId(productId, imageId);
            return CreatedAtAction(nameof(GetImageId), new { id = imageId }, image);
        }

        [HttpPut("{productId}/images/{imageId}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateImage([FromRoute] int imageId, [FromForm] ProductImageUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _productServices.UpdateImage(imageId, request);
            if (result == 0)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpDelete("{productId}/images/{imageId}")]
        public async Task<IActionResult> DeleteImage(int imageId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var image = await _productServices.RemoveImage(imageId);
            if (image == 0)
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}