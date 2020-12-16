using EComm.Data.Interfaces;
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
    } 
}
