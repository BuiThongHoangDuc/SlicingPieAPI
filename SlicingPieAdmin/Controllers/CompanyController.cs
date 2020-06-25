using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using SlicingPieAdmin.Helper;
using SlicingPieAdmin.Models;

namespace SlicingPieAdmin.Controllers
{

    public class CompanyController : Controller
    {
        SlicingPieApi _api = new SlicingPieApi();
        [Route("Companies")]
        public async Task<IActionResult> GetCompanies()
        {
            String token = HttpContext.Session.GetString("token");
            String action = "Error";
            HttpClient client = _api.Initial();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            HttpResponseMessage res = await client.GetAsync("api/Companies");
            if (res.IsSuccessStatusCode)
            {
                ViewData["Companies"] = JsonConvert.DeserializeObject<List<Company>>(res.Content.ReadAsStringAsync().Result);
            }
            else
            {
                return RedirectToAction(action, "Login");
            }

            return View();
        }
    }
}
