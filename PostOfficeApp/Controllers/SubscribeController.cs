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
        private readonly NewspaperDbContext _context;

        public SubscribeController(UserManager<ApplicationUser> manager, NewspaperDbContext context)
        {
            _context = context;
            _userManager = manager;
        }
        [HttpGet]
        public ActionResult Create(string id)
        {
            /* @ usage:make a subscribe
             * @ id:发刊号
             */
            var newspaper = _context.Newspaper.Where(c => c.Pno_number == id).FirstOrDefault();
            var order = new Orders();
            return View(new Tuple<Newspaper,Orders>(newspaper,order));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Subscribe([Bind("Ona","Ofen","Opeopel","Oaddress","Opo")] Orders myOrder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(myOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction("/");
            }
            return View();
            return Redirect("/");
        }
    }
}