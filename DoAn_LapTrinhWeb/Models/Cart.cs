using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
namespace DoAn_LapTrinhWeb.Model
{
    public class Cart
    {
        private readonly DbContext db = new DbContext();
        public Cart(int product_id)
        {
            Product_ID = product_id;
            var sp = db.Products.Find(product_id);
            Product_name = sp.product_name;
            Product_image = sp.image;
            Quantity = 1;
        }
        //Product ID
        public int Product_ID { get; set; }
        //Product Name
        public string Product_name { get; set; }
        //Product Image
        public string Product_image { get; set; }
        //Quantity
        public int Quantity { get; set; }
    }
}