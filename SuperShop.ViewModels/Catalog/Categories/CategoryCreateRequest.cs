using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperShop.ViewModels.Catalog.Categories
{
    public class CategoryCreateRequest
    {
        [Required(ErrorMessage = "Name is require")]
        public string Name { get; set; }

        public string SeoDescription { get; set; }
        public string SeoTitle { get; set; }
        public string SeoAlias { get; set; }
        public string LanguageId { get; set; }
    }
}