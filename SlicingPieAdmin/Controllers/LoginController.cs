using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SlicingPieAdmin.Helper;
using SlicingPieAdmin.Models;

namespace SlicingPieAdmin.Controllers
{
    public class LoginController : Controller
    {
        SlicingPieApi _api = new SlicingPieApi();

        [Route("Login")]
        public async Task<IActionResult> Login(string token)
        {
            HttpClient client = _api.Initial();
            client.DefaultRequestHeaders.Add("Authorization", token);
            HttpResponseMessage res = await client.PostAsync("api/Login", null);
            if (res.IsSuccessStatusCode)
            {
                var data = JsonConvert.DeserializeObject<Info>(res.Content.ReadAsStringAsync().Result);

            } else
            {
                Console.WriteLine("error");
            }

            return RedirectToAction("HomePage");
        }

        public IActionResult HomePage()
        {

            
            return View();
        }
    }
}
