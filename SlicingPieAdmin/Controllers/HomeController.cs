using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SlicingPieAdmin.Helper;
using SlicingPieAdmin.Models;

namespace SlicingPieAdmin.Controllers
{
    public class HomeController : Controller
    {
        SlicingPieApi _api = new SlicingPieApi();
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            HttpClient client = _api.Initial();
            ViewData["data"] = "Data is null";
            HttpResponseMessage res = await client.GetAsync("api/Companies");

            if(res.IsSuccessStatusCode)
            {
 
                ViewData["data"] = res.Content.ReadAsStringAsync().Result;
            }

            return View();

        }

    }
}
