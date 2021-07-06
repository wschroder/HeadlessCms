using ContentViewerApp.Models;
using ContentViewerApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ContentViewerApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly BlurbViewModel _blurbModel = new BlurbViewModel();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Blurb()
        {
            _blurbModel.Title = "The Last Knight";
            _blurbModel.Description = await GetCmsContent("safety-domains");
            return View(_blurbModel);
        }

        private async Task<string> GetCmsContent(string v)
        {
            const string pageSlug = "safety-domains";
            const string sectionSlug = "injuries";

            var provider = new ContentProvider();

            string content = await provider.GetSection(pageSlug, sectionSlug);

            return content; ;
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
