using SuperShop.ViewModels.Catalog.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperShop.ViewModels.Catalog.Products
{
    public class GetProductPagingRequest : PageResultBase
    {
        public string Key { get; set; }
        public string LanguageId { get; set; }
        public int? CategoryId { get; set; }
    }
}