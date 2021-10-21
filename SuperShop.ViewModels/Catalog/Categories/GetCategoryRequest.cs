using SuperShop.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperShop.ViewModels.Catalog.Categories
{
    public class GetCategoryRequest : PageResultBase
    {
        public string Keyword { get; set; }
        public string LanguageId { get; set; }
    }
}