using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperShop.ViewModels.Catalog.Products
{
    public class ProductCreateRequest
    {
        [Required(ErrorMessage = "Name is require")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Price is require")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "OriginalPrice is require")]
        public decimal OriginalPrice { get; set; }

        [Required(ErrorMessage = "Stock is require")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "Description is require")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Detail is require")]
        public string Detail { get; set; }

        [Required(ErrorMessage = "SeoDescription is require")]
        public string SeoDescription { get; set; }

        [Required(ErrorMessage = "SeoAlias is require")]
        public string SeoAlias { get; set; }

        [Required(ErrorMessage = "SeoTitle is require")]
        public string SeoTitle { get; set; }

        public string LanguageId { get; set; }
        public IFormFile ThumbnailImage { get; set; }
    }
}