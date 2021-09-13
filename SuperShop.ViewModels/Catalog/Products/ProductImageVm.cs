using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperShop.ViewModels.Catalog.Products
{
    public class ProductImageVm
    {
        public int Id { get; set; }
        public int productId { get; set; }
        public string imagePath { get; set; }
        public string Caption { get; set; }
        public bool IsDefault { get; set; }
        public long fileSize { get; set; }
        public DateTime dateCreate { get; set; }
        public int sortOrder { get; set; }
    }
}