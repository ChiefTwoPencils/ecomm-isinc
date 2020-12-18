using EComm.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace EComm.Web.Models
{
    public class ShoppingCart
    {
        public ShoppingCart()
        {
            LineItems = new List<LineItem>();
        }

        public List<LineItem> LineItems { get; set; }
        public string FormattedGrandTotal => $"{LineItems.Sum(i => i.TotalCost):C}";

        public static ShoppingCart GetFromSession(ISession session)
        {
            byte[] data;
            ShoppingCart cart = null;
            var b = session.TryGetValue(nameof(ShoppingCart), out data);
            if (b)
            {
                var json = Encoding.UTF8.GetString(data);
                cart = JsonSerializer.Deserialize<ShoppingCart>(json);
            }
            return cart ?? new ShoppingCart();
        }

        public static void StoreInSession(ShoppingCart cart, ISession session) 
        {
            var json = JsonSerializer.Serialize(cart);
            var data = Encoding.UTF8.GetBytes(json);
            session.Set(nameof(ShoppingCart), data);
        }

        public class LineItem
        {
            public Product Product { get; set; }
            public int Quantity { get; set; }
            public decimal TotalCost => Product.UnitPrice.Value * Quantity;
        }
    }
}
