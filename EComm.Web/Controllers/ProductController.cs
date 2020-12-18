using EComm.Data.Entities;
using EComm.Data.Interfaces;
using EComm.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EComm.Web.Controllers
{
    [Route("[controller]")]
    public class ProductController : Controller
    {
        private readonly IRepository _repo;
        private readonly IAuthorizationService _auth;

        public ProductController(IRepository repo, IAuthorizationService auth)
        {
            _repo = repo;
            _auth = auth;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var product = await _repo.GetProduct(id, true);            
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpGet("edit/{id}")]
        [Authorize(Policy = "AdminsOnly")]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _repo.GetProduct(id, true);
            if (product == null)
            {
                return BadRequest();
            }
            var model = new ProductEditViewModel
            {
                Id = product.Id,
                ProductName = product.ProductName,
                Package = product.Package,
                UnitPrice = product.UnitPrice,
                IsDiscontinued = product.IsDiscontinued,
                SupplierId = product.SupplierId,
                Supplier = product.Supplier,
                Suppliers = await _repo.GetAllSuppliers()
            };
            return View(model);
        }

        [HttpPost("edit/{id}")]
        [Authorize(Policy = "AdminsOnly")]
        [Authorize(Policy = "LessThan100")]
        public async Task<IActionResult> Edit(int id, ProductEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var original = await _repo.GetProduct(id);
                var authed = await _auth.AuthorizeAsync(User, original, "LessThan");
                if (!authed.Succeeded)
                {
                    return new ForbidResult();
                }
                var product = new Product
                {
                    Id = id,
                    ProductName = model.ProductName,
                    Package = model.Package,
                    UnitPrice = model.UnitPrice,
                    IsDiscontinued = model.IsDiscontinued,
                    SupplierId = model.SupplierId
                };
                
                await _repo.UpdateProduct(product);
                return RedirectToAction("Details", new { id });
            }
            model.Suppliers = await _repo.GetAllSuppliers();
            return View(model);
        }

        [HttpPost("addtocart")]
        public async Task<IActionResult> AddToCart(int id, int quantity)
        {
            var product = await _repo.GetProduct(id);
            if (product == null || quantity < 1)
            {
                return BadRequest();
            }
            var totalCost = product.UnitPrice * quantity;
            var msg = $"You added {quantity} {product.ProductName}(s) "
                + $"to your cart for a total cost of {totalCost:C}.";

            var cart = ShoppingCart.GetFromSession(HttpContext.Session);
            var lineItem = cart.LineItems.SingleOrDefault(i => i.Product.Id == id);
            if (lineItem == null)
            {
                cart.LineItems.Add(new ShoppingCart.LineItem
                {
                    Product = product,
                    Quantity = quantity
                });
            }
            else
            {
                lineItem.Quantity += quantity;
            }
            ShoppingCart.StoreInSession(cart, HttpContext.Session);
            return PartialView("_AddedToCart", msg);
        }

        public IActionResult Cart()
        {
            var cart = ShoppingCart.GetFromSession(HttpContext.Session);
            return View(new CartViewModel { Cart = cart });
        }

        [HttpPost("checkout")]
        public IActionResult Checkout(CartViewModel model)
        {
            if (ModelState.IsValid)
            {
                HttpContext.Session.Clear();
                return View("ThankYou");
            }
            model.Cart = ShoppingCart.GetFromSession(HttpContext.Session);
            return View("Cart", model);
        }
    } 
}
