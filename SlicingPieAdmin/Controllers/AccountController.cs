﻿using System;
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
  
    public class AccountController : Controller
    {
        SlicingPieApi _api = new SlicingPieApi();
        [Route("Accounts")]
        public async Task<IActionResult> GetAccount()
        {
            String token = HttpContext.Session.GetString("token");
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
