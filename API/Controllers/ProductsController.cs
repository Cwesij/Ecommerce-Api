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
            return await _storeContext.Products.ToListAsync();
        }

        [HttpGet("{id:int}")] // api/products/3
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
             return    await _storeContext.Products.FindAsync(id);
        }
    }
}