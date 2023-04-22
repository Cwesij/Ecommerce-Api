using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("BasketItems")]
    public class BasketItem
    {
        /*
            BasketItems will contain items that are products.
            it will have an Id property
            and a property to set the Quantity the user wants. i.e. 
            how many of these items are beeen stored.

            // relationships
            1. we need a relationship between the BasketItems and the Products been stored.
            i.e. we give it a navigation property. i.e. how does it get data from the product
                to the BasketItems.

            we will not see the product with its properties in the BasketItems
            but only the ProductId.
        */

        public int Id { get; set; }
        public int Quantity { get; set; }

        // navigation property (Product)
        public int ProductId { get; set; }
        public Product Product { get; set; }

        // navigation property (Basket)
        public int BasketId { get; set; }
        public Basket Basket { get; set; }

    }
}