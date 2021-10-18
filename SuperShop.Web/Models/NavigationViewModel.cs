using SuperShop.ViewModels.Systems.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperShop.Web.Models
{
    public class NavigationViewModel
    {
        public List<LanguageVm> Languages { get; set; }
        public string CurrentLanguageId { get; set; }
    }
}