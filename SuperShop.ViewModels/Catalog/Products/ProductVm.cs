using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperShop.ViewModels.Catalog.Products

{
    public class ProductVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal OriginalPrice { get; set; }
        public DateTime DateCreated { get; set; }
        public int Stock { get; set; }
        public string Description { get; set; }
        public string Detail { get; set; }
        public string SeoDescription { get; set; }
        public string SeoAlias { get; set; }
        public string SeoTitle { get; set; }
        public string LanguageId { get; set; }
        public string ThumbnailImage { get; set; }
        public List<string> Categories { get; set; } = new List<string>();
    }
}