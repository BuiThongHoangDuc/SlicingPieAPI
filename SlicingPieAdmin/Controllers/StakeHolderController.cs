using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SlicingPieAdmin.Helper;
using SlicingPieAdmin.Models;

namespace SlicingPieAdmin.Controllers
{
    public class StakeHolderController : Controller
    {
        SlicingPieApi _api = new SlicingPieApi();
        [Route("StakeHolders")]
        public async Task<IActionResult> GetStakeHolder()
        {

            String token = HttpContext.Session.GetString("token");
            String action = "Error";
            HttpClient client = _api.Initial();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            HttpResponseMessage res = await client.GetAsync("api/StackHolders");
            if (res.IsSuccessStatusCode)
            {
                ViewData["StakeHolders"] = JsonConvert.DeserializeObject<List<StakeHolderViewModel>>(res.Content.ReadAsStringAsync().Result);
            }
            else
            {
                return RedirectToAction(action, "Login");
            }

            return View();
        }
    }
}
