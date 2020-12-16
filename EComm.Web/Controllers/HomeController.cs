using EComm.Data.Interfaces;
using EComm.Web.Interfaces;
using EComm.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace EComm.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmailService _emailService;
        private readonly IRepository _repo;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IEmailService emailService, IRepository repo, ILogger<HomeController> logger)
        {
            _emailService = emailService;
            _repo = repo;
            _logger = logger;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            _emailService.SendEmail("robert.wilk@dss.ca.gov", "don't capture dependencies");
            var products = await _repo.GetAllProducts();
            return Json(products);
        }

        [HttpGet("privacy")]
        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet("error")]
        [HttpPost("error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
