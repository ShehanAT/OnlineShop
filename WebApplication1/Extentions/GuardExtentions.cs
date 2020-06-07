using Microsoft.WebApplication1.Entities.BasketAggregate;
using Microsoft.WebApplication1.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ardalis.GuardClauses
{// making a custom Guard Clause to check for null baskets 
    public static class BasketGuards
    {
        public static void NullBasket(this IGuardClause guardClause, int basketId, Basket basket)
        { // first param must be passed by default for Ardalis.GuardClauses to recognize the new guard clause
            if (basket == null)
                throw new BasketNotFoundException(basketId);
        }
    }
}
