using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using SlicingPieAdmin.Helper;
using SlicingPieAdmin.Models;

namespace SlicingPieAdmin.Controllers
{

    public class CompanyController : Controller
    {
        SlicingPieApi _api = new SlicingPieApi();


        private IDistributedCache redisCache;
        public CompanyController(IDistributedCache redisCache)
        {
            this.redisCache = redisCache;
        }

        [Route("Companies")]
        public async Task<IActionResult> GetCompanies()
        {
  
            String token = redisCache.GetString("token");
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


        [Route("Companies/delete/{id}")]
        public async Task<IActionResult> DeleteCompany(String id)
        {

            String token = redisCache.GetString("token");
            String action = "Error";
            HttpClient client = _api.Initial();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            HttpResponseMessage res = await client.DeleteAsync("api/Companies/" + id);
            if (!res.IsSuccessStatusCode)
            {
                return RedirectToAction(action, "Login");
            }

            return Redirect($"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/Companies");
        }





        [Route("Companies/{id}/Contribution")]
        public async Task<IActionResult> GetContribution(String id)
        {

            String token = redisCache.GetString("token");
            String action = "Error";
            HttpClient client = _api.Initial();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            HttpResponseMessage res = await client.GetAsync("api/Companies/"+id+"/Contribution");
            if (res.IsSuccessStatusCode)
            {
                ViewData["Contribution"] = JsonConvert.DeserializeObject<List<ContributionViewModel>>(res.Content.ReadAsStringAsync().Result);
            }
            else
            {
                return RedirectToAction(action, "Login");
            }

            return View();
        }

        [Route("Companies/{id}/stake-holder")]
        public async Task<IActionResult> GetStakeHolder(String id)
        {

            String token = redisCache.GetString("token");
            String action = "Error";
            HttpClient client = _api.Initial();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            HttpResponseMessage res = await client.GetAsync("api/Companies/"+id+"/stake-holder");
            if (res.IsSuccessStatusCode)
            {
                ViewData["StakeHolders"] = JsonConvert.DeserializeObject<List<StakeHolderInCompany>>(res.Content.ReadAsStringAsync().Result);
            }
            else
            {
                return RedirectToAction(action, "Login");
            }

            return View();
        }


        [Route("Companies/{id}")]
        [HttpPost]
        public async Task<IActionResult> UpdateCompany([FromForm] Company companyModel, String id)
        {
            companyModel.CompanyId = id;

            String token = redisCache.GetString("token");
            String action = "Error";
            HttpClient client = _api.Initial();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            HttpResponseMessage res = await client.PutAsJsonAsync("api/Companies/" + id, companyModel);
            if (!res.IsSuccessStatusCode)
            {
                return RedirectToAction(action, "Login");
            }

            return Redirect($"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/Companies");
        }


        [Route("Companies")]
        [HttpPost]
        public async Task<IActionResult> CreateCompany([FromForm] Company companyModel)
        {
            companyModel.CompanyId = "";

            String token = redisCache.GetString("token");
            String action = "Error";
            HttpClient client = _api.Initial();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            HttpResponseMessage res = await client.PostAsJsonAsync("api/Companies/", companyModel);
            if (!res.IsSuccessStatusCode)
            {
                return RedirectToAction(action, "Login");
            }

            return Redirect($"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/Companies");
        }

    }
}
