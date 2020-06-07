using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WebApplication1.Constants;
using Microsoft.WebApplication1.Identity;
using Microsoft.WebApplication1.Interfaces;
using Microsoft.WebApplication1.Pages.Basket;
using Microsoft.WebApplication1.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.WebApplication1.Pages.Shared.Components.BasketComponent
{
    public class Basket : ViewComponent
    {
        private readonly IBasketViewModelService _basketService;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public Basket(IBasketViewModelService basketService, SignInManager<ApplicationUser> signInManager)
        {
            _basketService = basketService;
            _signInManager = signInManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
            //even though method requires params if those params are not needed in the method they do not have to be typed out
        {   // create new basketComponentViewModel, sets itemCount and returns basketComponentViewModel
            var vm = new BasketComponentViewModel();
            vm.itemCount = (await GetBasketViewModelAsync()).Items.Sum(i => i.Quantity);
            return View(vm);
        }

        public async Task<BasketViewModel> GetBasketViewModelAsync()
        {
            if (_signInManager.IsSignedIn(HttpContext.User))
            {
                return await _basketService.GetOrCreateBasketForUser(User.Identity.Name);
            }
            string anonymousId = GetBasketIdFromCookie();
            if (anonymousId == null) return new BasketViewModel();
            return await _basketService.GetOrCreateBasketForUser(anonymousId);
        }

        public string GetBasketIdFromCookie()
        {
            if (Request.Cookies.ContainsKey(ApplicationConstants.BASKET_COOKIENAME))
            {
                return Request.Cookies[ApplicationConstants.BASKET_COOKIENAME];
            }
            return null;
        }
    }
}
