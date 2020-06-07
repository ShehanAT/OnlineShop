using Microsoft.WebApplication1.Entities.BasketAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
/* THIS IS THE PROBLEM, CATALOG ITEMS ARE NOT BEING INCLUDED TO THE BASKET SPEC*/
namespace Microsoft.WebApplication1.Specifications
{
    public sealed class BasketWithItemsSpecification : BaseSpecification<Basket>
    {
        public BasketWithItemsSpecification(int basketId)
            :base(b => b.id == basketId) // id is from BaseEntity
        {
            // this constructor is using the parent class's(baseSpecification<Basket>) constructor
            AddInclude(b => b.Items); // Items is a read-only collection of elements, adding basketItem to Items
        }

        public BasketWithItemsSpecification(string buyerId)
            :base(b => b.BuyerId == buyerId)
        {
            AddInclude(b => b.Items);
        }
    }
}
