using Core.Contracts;
using Core.Services;
using Core.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Web.ViewModels;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProfessionService professionService;
        private readonly ISchoolService schoolService;

        public HomeController(ILogger<HomeController> logger, IProfessionService professionService, ISchoolService schoolService)
        {
            _logger = logger;
            this.professionService = professionService;
            this.schoolService = schoolService;
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
