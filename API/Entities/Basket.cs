using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class Basket
    {
        /*  
            BuyerId: 
                the BuyerId is created to allow users to add items to the basket
                without signing in.
                so, we need to give them a randomly generated id
                so, we can keep track of whose basket it belongs to.

            Items:
                1. the list of items they have in their basket.
                2. we have to initialize the BasketItems to a new list whenever
                    we create a new basket.
                3. to help us prevent list not been defined errors.

            // methods .i.e. to help us in adding and removing items from the basket.

            AddItem():
                1. to add an item we need the Product we are adding.
                2. we need to know the Quantity to add as well.
                3. if we already have the item in the basket, 
                    then we will only be adding the Quantity we want.
                    else
                
                4. we will add the product as well as specifying the quantity we want.

            RemoveItem():
                1. we need to pass only the productId and the quantity to remove(reduce).

        */

        public int Id { get; set; }
        public string BuyerId { get; set; }
        public List<BasketItem> Items { get; set; } = new List<BasketItem>();

        // method to add items to the basket
        public void AddItem(Product product, int quantity)
        {
            // check if product is not in basket
            if(Items.All( item => item.ProductId != product.Id)) 
            {
                // add a new item to the list
                Items.Add(new BasketItem(){Product = product, Quantity = quantity});
            }

            // if item already exist, then adjust the quantity
            var existingItem = Items.FirstOrDefault( item => item.ProductId == product.Id);
            if(existingItem != null) existingItem.Quantity += quantity;
        }

        // method to remove items from the basket
        public void RemoveItem(int productId, int quantity)
        {
            // check if item in basket
            var existingItem = Items.FirstOrDefault( item => item.ProductId == productId);
            
            if(existingItem == null) return;
            // if item exist adjust only the quantity
            existingItem.Quantity -= quantity;

            // if item quantity is zero(0), remove item
            if(existingItem.Quantity == 0) Items.Remove(existingItem);
            
        }
    }
}