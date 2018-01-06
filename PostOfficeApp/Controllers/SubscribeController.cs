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
        [Route("/subscribe")]
        public ActionResult Subscribe()
        {
            /* @usage:make a subscribe
             * @ 
             */
            return View("/Home/Subscribe.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Subscribe(string q)
        {
            return Redirect("/");
        }
    }
}