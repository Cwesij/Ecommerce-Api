using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly StoreContext _storeContext;

        public ProductsController(StoreContext storeContext)
        {
            this._storeContext = storeContext;
        }

        [HttpGet] // api/products
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            var products = await _storeContext.Products.ToListAsync();
            return products;
        }

        [HttpGet("{id:int}")] // api/products/3
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _storeContext.Products.FindAsync(id);

            return product == null ? NotFound() : product;  
        }
    }
}