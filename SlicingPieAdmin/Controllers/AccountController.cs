using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SlicingPieAdmin.Controllers
{
    public class AccountController : Controller
    {
        [Route("Accounts")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
