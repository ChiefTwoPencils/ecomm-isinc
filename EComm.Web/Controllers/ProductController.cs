using EComm.Data.Entities;
using EComm.Data.Interfaces;
using EComm.Web.Models;
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

        public ProductController(IRepository repo)
            => _repo = repo;

        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var product = await _repo.GetProduct(id, true);
            return View(product);
        }

        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _repo.GetProduct(id, true);
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
        public async Task<IActionResult> Edit(int id, ProductEditViewModel model)
        {
            if (ModelState.IsValid)
            {
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
            var totalCost = product.UnitPrice * quantity;
            var msg = $"You added {quantity} {product.ProductName}(s) "
                + $"to your cart for a total cost of {totalCost:C}.";
            return PartialView("_AddedToCart", msg);
        }
    } 
}
