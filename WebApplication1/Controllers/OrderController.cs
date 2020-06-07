using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WebApplication1.Constants;
using Microsoft.WebApplication1.Entities.OrderAggregate;
using Microsoft.WebApplication1.Features;
using Microsoft.WebApplication1.Identity;
using Microsoft.WebApplication1.Interfaces;
using Microsoft.WebApplication1.Pages.Basket;

namespace Microsoft.WebApplication1.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize] // Controllers that mainly require Authentication still use Controller/Views(folder), others use Pages(folder)
    [Route("[controller]/[action]")]
    public class OrderController : Controller
    {
        private readonly IBasketService _basketService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IOrderService _orderService;
        private string _username = null;
        private readonly IBasketViewModelService _basketViewModelService;
        private readonly IMediator _mediator;
        // ApiDescription objects will be created for this controller
        public BasketViewModel basketModel { get; set; } = new BasketViewModel();
        public OrderController(IBasketService basketService, SignInManager<ApplicationUser> signInManager, IOrderService orderService, IBasketViewModelService basketViewModelService, IMediator mediator)
        {
            this._basketService = basketService;
            this._signInManager = signInManager;
            this._orderService = orderService;
            this._basketViewModelService = basketViewModelService;
            this._mediator = mediator;
        }

   /*     public IActionResult Index()
        {
            
            return View();
        }*/

        [HttpGet()]
        public async Task<IActionResult> MyOrders()
        {
            // User is from SecurityClaims class, which is made from HTTP request data(Name provided from HTTP request data)
            var viewModel = await _mediator.Send(new GetMyOrders(User.Identity.Name));

            return View(viewModel);
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> Detail(int orderId)
        {
            // User.Identity.Name contains username of authenticated user
            var viewModel = await _mediator.Send(new GetOrderDetails(User.Identity.Name, orderId));
            if(viewModel == null)
            {
                return BadRequest("No such order found for this user.");
            }
            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Checkout(Dictionary<string, int> items)
        {
            await SetBasketModelAsync();
            await _basketService.SetQuantities(basketModel.Id, items);
            await _orderService.CreateOrderAsync(basketModel.Id, new Address("1963 Mckay Ave", "Windsor", "ON", "Canada", "N9B0A1"));
            await _basketService.DeleteBasketAsync(basketModel.Id);
            return View("/Views/Order/CheckoutView.cshtml"); // redirect to current page
        }

        public async Task SetBasketModelAsync()
        {
            if (_signInManager.IsSignedIn(HttpContext.User))
            {
                basketModel = await _basketViewModelService.GetOrCreateBasketForUser(User.Identity.Name);
            }
            else
            {
                GetOrSetBasketCookieAndUsername();
                basketModel = await _basketViewModelService.GetOrCreateBasketForUser(_username);
            }
        }

        public void GetOrSetBasketCookieAndUsername()
        {
            if (Request.Cookies.ContainsKey(ApplicationConstants.BASKET_COOKIENAME))
            {
                _username = Request.Cookies[ApplicationConstants.BASKET_COOKIENAME];
            }
            if (_username != null) return;

            _username = Guid.NewGuid().ToString();
            var cookieOptions = new CookieOptions();
            cookieOptions.Expires = DateTime.Today.AddYears(5);
            Response.Cookies.Append(ApplicationConstants.BASKET_COOKIENAME, _username, cookieOptions);

        }

    }
}
