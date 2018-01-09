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
        private readonly OrdersDbContext _context2;

        public SubscribeController(UserManager<ApplicationUser> manager, NewspaperDbContext context,
            OrdersDbContext context2)
        {
            _context = context;
            _userManager = manager;
            _context2 = context2;
        }
        [HttpGet]
        [Authorize(Roles ="Client")]
        public ActionResult Create(string id)
        {
            /* @ usage:make a subscribe
             * @ id:发刊号
             */
            var newspaper = _context.Newspaper.Where(c => c.Pno_number == id).FirstOrDefault();
            var order = new Orders();
            var newsAndOrder = new OrdersViewModel() { Item1 = newspaper, Item2 = order };
            return View(newsAndOrder);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> Create(string id, 
            [Bind("Item2")]
            OrdersViewModel myOrder)
        {
            var tmporder = myOrder.Item2;
            var newspaper = _context.Newspaper.Where(c => c.Pno_number == id).FirstOrDefault();
            var order = new Orders()
            {
                Ona = tmporder.Ona,
                Ofen = tmporder.Ofen,
                Oaddress = tmporder.Oaddress,
                Gpo = tmporder.Gpo,
                Gte = tmporder.Gte,
                Gna = tmporder.Gna
            };
            order.Boolpay = 0;
            order.Last_time = 1;
            order.Onumber = DateTime.Now.ToFileTimeUtc().ToString();
            order.Oprice = newspaper.Ppr * order.Ofen;
            order.Ostart_year = DateTime.Now.Year;
            order.Ona = newspaper.Pno_number;

            string userName = HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            var uid = user.Id;
            order.Opeople = uid;
            if (ModelState.IsValid)
            {
                _context2.Add(order);
                await _context2.SaveChangesAsync();
                return Redirect("/Manage/Item");
            }
            return View(new OrdersViewModel() { Item1=newspaper,Item2=order});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> Pay([Bind("Onumber")]Orders order)
        {
            var Onumber = order.Onumber;
            if(_context2.Orders.FirstOrDefault(c=>c.Onumber.Equals(Onumber)) == null)
            {
                return NotFound();
            }
            else
            {
                var norder = _context2.Orders.FirstOrDefault(c => c.Onumber.Equals(Onumber));
                norder.Boolpay = 1;
                _context2.Orders.Update(norder);
                await _context2.SaveChangesAsync();
                return Redirect("/Manage/Item");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        [Route("/Subscribe/PayConfirm")]
        public IActionResult Payconfirm([Bind("Onumber")]Orders order)
        {
            var Onumber = order.Onumber;
            if (_context2.Orders.FirstOrDefault(c => c.Onumber.Equals(Onumber)) == null)
            {
                return NotFound();
            }
            else
            {
                var norder = _context2.Orders.FirstOrDefault(c => c.Onumber.Equals(Onumber));
                norder.Boolpay = 2;
                _context2.Orders.Update(norder);
                _context2.SaveChanges();
                return Redirect("/Manage/Item");
            }
        }
    }
}