using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Http;
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

        [HttpGet(Name = "GetBasket")]
        // endpoint to get a basket
        public async Task<ActionResult<BasketDto>> GetBasket()
        {
            // retrieve basket
            var basket = await RetrieveBasket();

            // check if basket found
            if (basket == null) return NotFound();

            // map basket to basketDto
            return MapBasketToDto(basket);
        }


        // endpoint to add a basket
        [HttpPost] // api/basket?productId=1&quantity=4
        public async Task<ActionResult<BasketDto>> AddItemToBasket(int productId, int quantity)
        {
            // retrieve basket
            var basket = await RetrieveBasket();

            // if basket does not exist, then we create a new basket for the user.
            if(basket == null) basket = await CreateBasket();

            // retrieve the product
            var product = await _context.Products.FindAsync(productId);

            // check if product is found
            if(product == null) return NotFound();

            // add item to basket
            basket.AddItem(product, quantity);

            // check if changes has been saved
            var result = await _context.SaveChangesAsync() > 0;

            // return a success status code.
            if(result) return CreatedAtRoute("GetBasket", MapBasketToDto(basket));

            // if there are any errors 
            return BadRequest(new ProblemDetails{Title="Problem saving item to basket"});
        }

       

        // endpoint to remove a basket
        [HttpDelete]
        public async Task<ActionResult> RemoveItem(int productId, int quantity)
        {
            // retrieve basket to remove
            var basket = await RetrieveBasket();

            // check if basket exist
            if(basket == null) return NotFound();

            // remove item from basket
            basket.RemoveItem(productId, quantity);

            // save changes to database
            var result = await _context.SaveChangesAsync() > 0;

            // return success code
            if(result) return Ok();

            // return error if false
            return BadRequest(new ProblemDetails{Title="Problem removing item from basket"});

        }




        // methods 

        // method to retrieve basket
        private async Task<Basket> RetrieveBasket()
        {
            var basket = await _context.Baskets
                    // retrieve the items
                    .Include(i => i.Items)
                    // retrieve the product itself
                    .ThenInclude(p => p.Product)
                    // retrieve the buyer id using cookies
                    .FirstOrDefaultAsync(item => item.BuyerId == Request.Cookies["buyerId"]);
            return basket;
        }

        // method to create basket
        private async Task<Basket> CreateBasket()
        {
            // create a guid as the buyerId
            var buyerId = Guid.NewGuid().ToString();

            // create the cookie
            var cookieOptions = new CookieOptions{
                IsEssential = true,
                Expires = DateTime.Now.AddDays(30)
            };

            // add the cookie to the response
            Response.Cookies.Append("buyerId", buyerId, cookieOptions);

            // initialize the buyerId property
            var basket = new Basket{BuyerId = buyerId};

            // add the basket to the basket table 
            await _context.Baskets.AddAsync(basket);

            return basket;
        }

        // method to map basket to basket Dto
        private BasketDto MapBasketToDto(Basket basket)
        {
            return new BasketDto()
            {
                Id = basket.Id,
                BuyerId = basket.BuyerId,
                Items = basket.Items.Select(item => new BasketItemDto
                {
                    ProductId = item.ProductId,
                    Name = item.Product.Name,
                    Price = item.Product.Price,
                    PictureUrl = item.Product.PictureUrl,
                    Type = item.Product.Type,
                    Brand = item.Product.Brand,
                    Quantity = item.Quantity
                }).ToList()
            };
        }
    }
}