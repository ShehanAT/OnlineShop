using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.WebApplication1.ViewModels
{
    public class CatalogItemViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string pictureUri { get; set; }
        public decimal price { get; set; }
       
    }
}
