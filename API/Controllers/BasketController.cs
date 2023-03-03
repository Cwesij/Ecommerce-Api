using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class BasketController : BaseApiController
    {
        private readonly StoreContext _context;
        public BasketController(StoreContext context)
        {
            _context = context;          
        }

        public async Task<ActionResult<Basket>> GetBasket()
        {
            var basket = await _context.Baskets
            // retrieve the items
            .Include( i => i.Items)
            // retrieve the product
            .ThenInclude( p => p.Product)
            // retrieve the buyerId using the cookie
            .FirstOrDefaultAsync( x => x.BuyerId == Request.Cookies["buyerId"]);

            // check if the basket is not found or is null
            if(basket == null) return NotFound();

            return basket;
        }
    }
}