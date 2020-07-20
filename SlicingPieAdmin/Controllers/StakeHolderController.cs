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
    public class StakeHolderController : Controller
    {
        SlicingPieApi _api = new SlicingPieApi();
        private IDistributedCache redisCache;
        public StakeHolderController(IDistributedCache redisCache)
        {
            this.redisCache = redisCache;
        }
        [Route("StakeHolders")]
        public async Task<IActionResult> GetStakeHolder()
        {
    
            String action = "Error";
            HttpClient client = _api.Initial();

            String token = redisCache.GetString("token");
            

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
