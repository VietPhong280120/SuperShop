using SuperShop.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperShop.ViewModels.Systems.User
{
    public class GetUserRequest : PageResultBase
    {
        public string KeyWord { get; set; }
    }
}