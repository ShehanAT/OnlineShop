using Microsoft.WebApplication1.Pages.Basket;
using Microsoft.WebApplication1.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.WebApplication1.Interfaces
{
    public interface IBasketViewModelService
    {
        public Task<BasketViewModel> GetOrCreateBasketForUser(string username);
    }
}
