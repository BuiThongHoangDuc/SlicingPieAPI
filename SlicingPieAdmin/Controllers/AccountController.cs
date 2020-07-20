using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using SlicingPieAdmin.Helper;
using SlicingPieAdmin.Models;

namespace SlicingPieAdmin.Controllers
{
  
    public class AccountController : Controller
    {
        private IDistributedCache redisCache;
        public AccountController(IDistributedCache redisCache)
        {
            this.redisCache = redisCache;
        }

        SlicingPieApi _api = new SlicingPieApi();
        [Route("Accounts")]
        public async Task<IActionResult> GetAccount()
        {
            String token = redisCache.GetString("token");
            String action = "Error";
            HttpClient client = _api.Initial();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            HttpResponseMessage res = await client.GetAsync("api/Accounts");
            if (res.IsSuccessStatusCode)
            {
                 ViewData["Accounts"] = JsonConvert.DeserializeObject<List<AccountViewModel>>(res.Content.ReadAsStringAsync().Result);
            }
            else
            {
                return RedirectToAction(action, "Login");
            }

            return View();
        }
    }
}
