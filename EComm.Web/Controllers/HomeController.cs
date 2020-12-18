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
        public IActionResult Index() => View();

        [HttpGet("privacy")]
        public IActionResult Privacy() => View();

        [HttpGet("error")]
        [HttpPost("error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet("clienterror")]
        [HttpPost("clienterror")]
        public IActionResult ClientError(int statusCode)
        {
            ViewBag.Message = statusCode switch
            {
                400 => "Bad Request (400)",
                404 => "Not Found (404)",
                418 => "I'm a teapot (418)",
                _ => $"Unchecked error ({statusCode})"
            };
            return View();
        }
    }
}
