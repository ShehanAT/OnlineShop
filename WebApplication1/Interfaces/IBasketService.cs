using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.WebApplication1.Interfaces
{
    public interface IBasketService
    {
        Task<int> GetBasketItemCountAsync(string username);
        Task TransferBasketAsync(string anonymouseId, string username);
        Task AddItemToBasket(int basketId, int catalogItemId, decimal price, int quantity = 1);
        // set the quantities dictionary
        Task SetQuantities(int basketId, Dictionary<string, int> quantities);
        Task DeleteBasketAsync(int basketId);
    }
}
