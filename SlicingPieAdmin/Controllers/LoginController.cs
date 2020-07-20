using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using SlicingPieAdmin.Enums;
using SlicingPieAdmin.Helper;
using SlicingPieAdmin.Models;

namespace SlicingPieAdmin.Controllers
{
    public class LoginController : Controller
    {
        SlicingPieApi _api = new SlicingPieApi();
        private IDistributedCache redisCache;

        public LoginController(IDistributedCache cache)
        {
            this.redisCache = cache;
        }

        [Route("Login")]
        public async Task<IActionResult> Login(string token)
        {
            String action = "Error";

            HttpClient client = _api.Initial();

            client.DefaultRequestHeaders.Add("Authorization", token);
      
            HttpResponseMessage res = await client.PostAsync("api/Login", null);
            if (res.IsSuccessStatusCode)
            {
                var data = JsonConvert.DeserializeObject<Info>(res.Content.ReadAsStringAsync().Result);
                if(data.Role == (int) Role.ADMIN)
                {
                    action = "HomePage";
                    redisCache.SetString("token", data.Token);
                }

            } else
            {
                return RedirectToAction(action, "Login");
            }

            return RedirectToAction(action);
        }

        public IActionResult HomePage()
        {
            return View();
        }

        [Route("LogOut")]
        public async Task<IActionResult> LogOutAsync()
        {
            redisCache.Remove("token");
            return RedirectToAction("index", "Home");
        }

    }
}
