using EComm.Data.Entities;
using EComm.Data.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EComm.Web.Api
{
    [Route("api/product")]
    [ApiController]
    public class ProductApiController : ControllerBase
    {
        private readonly IRepository _repo;
        private readonly ILogger<ProductApiController> _logger;

        public ProductApiController(IRepository repo, ILogger<ProductApiController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            var products = await _repo.GetAllProducts(true);
            return products.ToList();
        }

        [HttpPost("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _repo.GetProduct(id, true);
            if (product == null)
            {
                return NotFound();
            }
            return product;
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (product.Id != id)
            {
                return BadRequest();
            }
            await _repo.UpdateProduct(product);
            return NoContent();
        }
    }
}
