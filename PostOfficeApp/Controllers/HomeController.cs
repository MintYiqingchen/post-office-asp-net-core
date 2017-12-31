using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PostOfficeApp.Models;

namespace PostOfficeApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [Route("/Home/search")]
        public JsonResult Search(string q)
        {
            JObject returnContent = new JObject();
            IEnumerable<string> keys = q.ToLower().Split();
            foreach (var s in keys)
            {
                if (Regex.IsMatch(s, @"^[0-9]+-[0-9]+$"))
                {
                    returnContent = new JObject(
                        new JProperty("results",
                            new JArray(
                                new JObject(new JProperty("title", "test title"), new JProperty("url", "/test"))
                            )
                        ));
                    return new JsonResult(returnContent);
                }
                else
                {
                    /* todo :search in database*/
                    returnContent = new JObject(
                        new JProperty("results",
                            new JArray(
                                new JObject(new JProperty("title", "test title"), new JProperty("url", "/test"))
                            )
                        ));
                    break;
                }
            }
            return new JsonResult(returnContent);
        }
    }
}
