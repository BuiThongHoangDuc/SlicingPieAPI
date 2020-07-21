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

        [Route("Accounts")]
        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromForm] AccountRegister account)
        {

            String token = redisCache.GetString("token");
            String action = "Error";
            HttpClient client = new HttpClient();
            Register res = new Register();
            res.email = account.EmailAccount;
            res.password = account.Password;
            res.returnSecureToken = true;


            HttpResponseMessage response = await client.PostAsJsonAsync(
                "https://identitytoolkit.googleapis.com/v1/accounts:signUp?key=AIzaSyAK2LGTJBlGvLvPAH9vz0XRGZOL71O0oQk", 
                res);
            if (!response.IsSuccessStatusCode)
            {
                return RedirectToAction(action, "Login");
            } else if (response.IsSuccessStatusCode)
            {
                client = _api.Initial();
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                response = await client.PostAsJsonAsync(
                "api/Accounts", account);
                if(!response.IsSuccessStatusCode)
                {
                    return RedirectToAction(action, "Login");
                }

            }

            return RedirectToAction("GetAccount");

        }


        [Route("Accounts/{id}")]
        [HttpPost]
        public async Task<IActionResult> UpdateAccount([FromForm] AccountViewModel account, String id)
        {

            account.StatusId = "";

            String token = redisCache.GetString("token");
            String action = "Error";
            HttpClient client = _api.Initial();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            HttpResponseMessage res = await client.PutAsJsonAsync("api/Accounts/" + id, account);
            if (!res.IsSuccessStatusCode)
            {
                return RedirectToAction(action, "Login");
            }

            return RedirectToAction("GetAccount");
        }

        [Route("Accounts/delete/{id}")]
        public async Task<IActionResult> DeleteAccount(String id)
        {

            String token = redisCache.GetString("token");
            String action = "Error";
            HttpClient client = _api.Initial();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            HttpResponseMessage res = await client.DeleteAsync("api/Accounts/" + id);
            if (!res.IsSuccessStatusCode)
            {
                return RedirectToAction(action, "Login");
            }

            return RedirectToAction("GetAccount");
        }

    }
}
