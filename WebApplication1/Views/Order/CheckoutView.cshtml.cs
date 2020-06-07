using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.WebApplication1.Views.Order
{
    public class CheckoutView : ViewComponent
    {
        public CheckoutView()
        {

        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("CheckoutView");
        }
    }
}
