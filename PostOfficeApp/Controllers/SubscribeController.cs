using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PostOfficeApp.Models;


namespace PostOfficeApp.Controllers
{
    [Authorize]
    public class SubscribeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public SubscribeController(UserManager<ApplicationUser> manager)
        {
            _userManager = manager;
        }
        [HttpGet]
        public ActionResult Create(string newspaperid)
        {
            /* @ usage:make a subscribe
             * @ id:发刊号
             */
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Subscribe([Bind("Ona","Ofen","Opeopel","Oaddress","Opo")] Orders myOrder)
        {
            return Redirect("/");
        }
    }
}