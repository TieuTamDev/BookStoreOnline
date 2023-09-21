using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookStoreOnline.Models;

namespace BookStoreOnline.Models
{
    public class CartItem
    {
        BookStoreEntities db = new BookStoreEntities();
        public int ProductID { get; set; }
        public string NamePro { get; set; }
        public string ImagePro { get; set; }
        public decimal Price { get; set; }
        public int Number { get; set; }


        public decimal FinalPrice()
        {
            return Number * Price;
        }
        public CartItem(int ProductID)
        {
            this.ProductID = ProductID;
            var productDB = db.Product.Single(s => s.ProductID == this.ProductID);
            this.NamePro = productDB.ProductName;
            this.ImagePro = productDB.ImageProd;
            this.Price = (decimal)productDB.Price;
            this.Number = 1;
        }
    }
}