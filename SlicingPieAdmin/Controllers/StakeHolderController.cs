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

        [Route("StakeHolders/{CompanyId}/{AccountId}")]
        public async Task<IActionResult> GetStakeHolderDetail(String CompanyId, String AccountId)
        {

            String action = "Error";
            HttpClient client = _api.Initial();

            String token = redisCache.GetString("token");


            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            HttpResponseMessage res = await client.GetAsync("api/StackHolders/"+CompanyId+"/"+AccountId);
            if (res.IsSuccessStatusCode)
            {
                ViewData["StakeHolders"] = JsonConvert.DeserializeObject<StakeHolderViewModel>(res.Content.ReadAsStringAsync().Result);
            }
            else
            {
                return RedirectToAction(action, "Login");
            }

            return View();
        }



        [Route("StakeHolders/{CompanyId}/{AccountId}")]
        [HttpPost]
        public async Task<IActionResult> UpdateStakeHolder(String CompanyId, String AccountId, StakeHolderViewModel stakeholder)
        {

            stakeholder.CompanyId = CompanyId;
            stakeholder.AccountId = AccountId;

            String token = redisCache.GetString("token");
            String action = "Error";
            HttpClient client = _api.Initial();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            HttpResponseMessage res = await client.PutAsJsonAsync("api/StackHolders/" +CompanyId+ "/" + AccountId, stakeholder);
            if (!res.IsSuccessStatusCode)
            {
                return RedirectToAction(action, "Login");
            }

            return RedirectToAction("GetStakeHolderDetail", new { CompanyId = CompanyId, AccountId = AccountId});
        }

        [Route("StakeHolders/{id}")]
        [HttpPost]
        public async Task<IActionResult> CreateStakeHolder([FromForm] StakeHolderViewModel stakeholder, String id)
        {

            stakeholder.CompanyId = id;

            String token = redisCache.GetString("token");
            String action = "Error";
            HttpClient client = _api.Initial();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            HttpResponseMessage res = await client.PostAsJsonAsync("api/StackHolders/company/"+id, stakeholder);
            if (!res.IsSuccessStatusCode)
            {
                return RedirectToAction(action, "Login");
            }

            return RedirectToAction("GetStakeHolder", "Company", new { id = id });
        }




    }
}
