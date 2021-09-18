using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperShop.ViewModels.Common
{
    public class SelectItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool Selected { get; set; }

        public Object Select()
        {
            throw new NotImplementedException();
        }
    }
}