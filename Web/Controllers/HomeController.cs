using Core.Contracts;
using Core.Services;
using Core.ViewModels.Home;
using Infrastructure.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Web.ViewModels;
using Core.ViewModels.Contacts;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProfessionService professionService;
        private readonly ISchoolService schoolService;
        private readonly IContactService _contactService;
        private readonly IHomeService homeService;

        public HomeController(ILogger<HomeController> logger, IProfessionService professionService, ISchoolService schoolService, IContactService contactService, IHomeService homeService)
        {
            _logger = logger;
            this.professionService = professionService;
            this.schoolService = schoolService;
            _contactService = contactService;
            this.homeService = homeService;
        }

        public async Task<IActionResult> Index(int? pendingApproval)
        {
            var professions = await professionService.GetRandomThreeAsync();
            var schools = await schoolService.GetRandomThreeAsync();
            var model = new HomeIndexVm
            {
                Professions = professions,
                Schools = schools
            };
            ViewBag.PendingApproval = pendingApproval == 1;
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View(new ContactFormVm());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(ContactFormVm model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _contactService.SaveMessageAsync(
                model.Name,
                model.Email,
                model.Message);

            TempData["Success"] = "Съобщението беше изпратено успешно!";
            return RedirectToAction(nameof(Contact));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public async Task<IActionResult> Search(string query)
        {
            var results = await homeService.SearchAsync(query);
            return View(results);
        }

    }
}
