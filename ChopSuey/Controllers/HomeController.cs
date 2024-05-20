using ChopSuey.Areas.Identity.Data;
using ChopSuey.Contracts;
using ChopSuey.Data;
using ChopSuey.Models;
using ChopSuey.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ChopSuey.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DbWebChopSuey _db;
        private readonly IPdfService _pdfService;
        private readonly UserManager<ApplicationUser> _userManager;
       

        public HomeController(ILogger<HomeController> logger, DbWebChopSuey db, IPdfService pdfService, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _db = db;
            _pdfService = pdfService;
            _userManager = userManager;
       
        }

        public IActionResult Index()
        {
            _db.Database.EnsureCreated();
            return View();
        }


        public async Task<IActionResult> GeneratePdf(string userId)
        {
            var pdfBytes = await _pdfService.CreatePdfAsync(userId);

            return File(pdfBytes, "application/pdf", "UserInformation.pdf");
        }

       
      
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}