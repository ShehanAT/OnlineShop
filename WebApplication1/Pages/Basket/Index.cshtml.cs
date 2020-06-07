using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.WebApplication1.Constants;
using Microsoft.WebApplication1.Identity;
using Microsoft.WebApplication1.Interfaces;
using Microsoft.WebApplication1.Services;
using Microsoft.WebApplication1.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.WebApplication1.Pages.Basket
{
    // only view files(.cshtml) with @page need coresponding PageModel classes
    public class MainIndexModel : PageModel
    {
        private readonly IBasketService _basketService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private string _username = null;
        private readonly IBasketViewModelService _basketViewModelService;
        public BasketViewModel basketModel { get; set; } = new BasketViewModel();
        public MainIndexModel(IBasketService basketService, IBasketViewModelService basketViewModelService,
            SignInManager<ApplicationUser> signInManager)
        {
            _basketService = basketService;
            _basketViewModelService = basketViewModelService;
            _signInManager = signInManager;
        }

        

        public async Task OnGet()
        {
            await SetBasketModelAsync();
        }

        private async Task SetBasketModelAsync()
        {
            if (_signInManager.IsSignedIn(HttpContext.User))
            {
                basketModel = await _basketViewModelService.GetOrCreateBasketForUser(User.Identity.Name);
            }
            else
            {  // user is not signed in, get cookie to use as username then get/create basket for user
                GetOrSetBasketCookieAndUsername();
                basketModel = await _basketViewModelService.GetOrCreateBasketForUser(_username);
            }
        }

        public async Task<IActionResult> OnPost(CatalogItemViewModel product)
        {
            if(product.id == 0)
            {
                return Page();
            }
            await SetBasketModelAsync();
            await _basketService.AddItemToBasket(basketModel.Id, product.id, product.price);
            await SetBasketModelAsync();
            return Page();
        }

        public async Task OnPostUpdate(Dictionary<string, int> items)
        {   // return type of Task on async methods === void on non-async methods
            await SetBasketModelAsync();
            await _basketService.SetQuantities(basketModel.Id, items);
            await SetBasketModelAsync();
        
        }

        private void GetOrSetBasketCookieAndUsername()
        {
            if (Request.Cookies.ContainsKey(ApplicationConstants.BASKET_COOKIENAME))
            {
                // if basket cookie is not empty/null assign its value to username var 
                _username = Request.Cookies[ApplicationConstants.BASKET_COOKIENAME];
            }
            if (_username != null) return;
            _username = Guid.NewGuid().ToString(); // represents a globally unique identifier 
            var cookieOptions = new CookieOptions { IsEssential = true }; // set new cookie 
            cookieOptions.Expires = DateTime.Today.AddYears(10); // add current dateTime to cookie
            // new username will be assigned to basket cookie 
            Response.Cookies.Append(ApplicationConstants.BASKET_COOKIENAME, _username, cookieOptions);
        }

    }
}
