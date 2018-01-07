using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PostOfficeApp.Models;
using PostOffice.Models;
using Microsoft.EntityFrameworkCore;
using PostOfficeApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace PostOfficeApp.Controllers
{   
    public enum KIND
    {
        NEWSPAPER,MAGZINE,ALL
    };

    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context1;
        private readonly NewspaperDbContext _context2;
        private readonly IAuthorizationService _authorizationService;
        private readonly UserManager<ApplicationUser> _userManager;
        public HomeController(
            ApplicationDbContext context1, NewspaperDbContext context2,
            IAuthorizationService authorizationService,UserManager<ApplicationUser> userManager)
        {
            _context1 = context1;
            _context2 = context2;
            _authorizationService = authorizationService;
            _userManager = userManager;
        }

        // http get
        public IActionResult Index()
        {
            List<string> labels = _context2.Newspaper.FromSql("select distinct labels from newspaper")
                .Select(c=>c.Labels).ToList();
            var content = new Dictionary<string, IEnumerable<Newspaper>>();
            foreach(var label in labels)
            {
                var temp = _context2.Newspaper.Where(c=>c.Labels==label).ToList();
                content.Add(label, temp.Take(Math.Min(temp.Count(),12)));
            }
            return View(content);
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

        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var newspaper = await _context2.Newspaper.SingleOrDefaultAsync(m => m.Pno_number == id);
            if (newspaper == null)
            {
                return NotFound();
            }
            var editModel = Model_to_viewModel(newspaper);
            return View(editModel);
        }
        [Route("/Home/search")]
        public JsonResult Search(string q="")
        {
            /*@usage: 返回走索栏需要的json结果
             * @q:搜索关键词
             * @k:见KIND定义
             */
            JObject returnContent = new JObject();
            IEnumerable<string> keys = q.ToLower().Split(' ');
            List<Newspaper> newspaper_list = new List<Newspaper>();
            foreach (var s in keys)
            {
                if (Regex.IsMatch(s, @"^[0-9]+-[0-9]+$"))
                {
                    //根据报刊发行号搜索
                    newspaper_list = new List<Newspaper>();
                    var query = from rows in _context2.Newspaper
                                where rows.Pno_number == q
                                select new
                                {
                                    rows.Pno_number,
                                    rows.Pna,
                                    rows.Ppr,
                                    rows.Pdw,
                                    rows.Ptype,
                                    rows.Total_sell_out,
                                    rows.Labels
                                };
                    foreach (var item in query)
                    {
                        Newspaper temp_newspaper = new Newspaper
                        {
                            Pno_number = item.Pno_number,
                            Pna = item.Pna,
                            Ppr = item.Ppr,
                            Pdw = item.Pdw,
                            Ptype = item.Ptype,
                            Total_sell_out = item.Total_sell_out,
                            Labels = item.Labels
                        };
                        newspaper_list.Add(temp_newspaper);
                    }

                    if (newspaper_list.Count() != 0)
                    {
                        returnContent = new JObject(
                        new JProperty("results",
                            new JArray(
                                //返回的url为/test/pno
                                from p in newspaper_list
                                select new JObject(
                                    new JProperty("title", p.Pna),
                                    new JProperty("url",
                                    System.Web.HttpUtility.UrlEncode($"Home/details/{p.Pno_number}", System.Text.Encoding.UTF8))
                                )
                            )
                        ));
                    }
                    else
                    {
                        returnContent = new JObject(
                        new JProperty("results",
                            new JArray(
                                //返回的url为/test/pno
                                new JObject(new JProperty("title", "暂无结果"))
                            )
                        ));
                    }
                    return new JsonResult(returnContent);
                }
                else
                {
                    //根据报刊名搜索
                    var query = from rows in _context2.Newspaper
                                    where rows.Pna.Contains(q)
                                    select new
                                    {
                                        rows.Pna,
                                        rows.Ppr,
                                        rows.Pdw,
                                        rows.Ptype,
                                        rows.Total_sell_out,
                                        rows.Labels
                                    };

                    foreach (var item in query)
                    {
                        Newspaper temp_newspaper = new Newspaper
                        {
                            Pna = item.Pna,
                            Ppr = item.Ppr,
                            Pdw = item.Pdw,
                            Ptype = item.Ptype,
                            Total_sell_out = item.Total_sell_out,
                            Labels = item.Labels
                        };
                        newspaper_list.Add(temp_newspaper);
                    }
                }
            }
            if (newspaper_list.Count() != 0)
            {
                returnContent = new JObject(
                   new JProperty("results",
                       new JArray(
                           from p in newspaper_list
                           select new JObject(
                               new JProperty("title", p.Pna),
                               new JProperty("url", 
                               System.Web.HttpUtility.UrlEncode($"Home/details/{p.Pno_number}", System.Text.Encoding.UTF8))
                           )
                       )
                   ));
            }
            else
            {
                returnContent = new JObject(
                   new JProperty("results",
                       new JArray(
                           new JObject(new JProperty("title", "暂无结果"))
                       )
                   ));
            }
            return new JsonResult(returnContent);
        }
        [Route("/Home/items")]
        public ActionResult Items(string q="",KIND k=KIND.ALL)
        {
            /* @usage：重定向到条目网页
             * @q:搜索关键词
             * @k:见KIND定义
             */
            IEnumerable<string> keys = q.ToLower().Split(' ');
            string str_kind = k.ToString();
            var newspaper_list = new List<Newspaper>();
            //搜索对象为报纸
            if (str_kind == "NEWSPAPER")
            {
                foreach (var s in keys)
                {
                    var query = from rows in _context2.Newspaper
                                where rows.Labels == str_kind && rows.Ptype == s
                                select new
                                {
                                    rows.Pna,
                                    rows.Ppr,
                                    rows.Pdw,
                                    rows.Ptype,
                                    rows.Total_sell_out,
                                    rows.Labels
                                };
                    List<Newspaper> temp_newspaper_list = new List<Newspaper>();
                    foreach (var item in query)
                    {
                        Newspaper temp_newspaper = new Newspaper
                        {
                            Pna = item.Pna,
                            Ppr = item.Ppr,
                            Pdw = item.Pdw,
                            Ptype = item.Ptype,
                            Total_sell_out = item.Total_sell_out,
                            Labels = item.Labels
                        };
                        newspaper_list.Add(temp_newspaper);
                    }
                }
                IEnumerable<Newspaper> result = newspaper_list;
                ViewData["kind"] = k;
                return View(result);
            }
            else
            {
                List<Newspaper> temp_newspaper_list = new List<Newspaper>();

                //先筛选出对应标签的条目，再一个个筛选发刊类型
                foreach (var s in keys)
                {
                    //筛选出符合类型的
                    if (!Regex.IsMatch(s, @"^\w+/\w+/\w+$"))
                    {
                        var query2 = from rows in _context2.Newspaper
                                     where rows.Ptype == s
                                     select new
                                     {
                                         rows.Pna,
                                         rows.Ppr,
                                         rows.Pdw,
                                         rows.Ptype,
                                         rows.Total_sell_out,
                                         rows.Labels
                                     };

                        foreach (var item in query2)
                        {
                            Newspaper temp_newspaper = new Newspaper
                            {
                                Pna = item.Pna,
                                Ppr = item.Ppr,
                                Pdw = item.Pdw,
                                Ptype = item.Ptype,
                                Total_sell_out = item.Total_sell_out,
                                Labels = item.Labels
                            };
                            temp_newspaper_list.Add(temp_newspaper);
                        }
                    }

                }
                foreach (var s in keys)
                {
                    //筛选出符合发刊周期的
                    if (Regex.IsMatch(s, @"^\w+/\w+/\w+$"))
                    {
                        var query2 = from rows in temp_newspaper_list
                                     where rows.Labels == s
                                     select new
                                     {
                                         rows.Pna,
                                         rows.Ppr,
                                         rows.Pdw,
                                         rows.Ptype,
                                         rows.Total_sell_out,
                                         rows.Labels
                                     };

                        foreach (var item in query2)
                        {
                            Newspaper temp_newspaper = new Newspaper
                            {
                                Pna = item.Pna,
                                Ppr = item.Ppr,
                                Pdw = item.Pdw,
                                Ptype = item.Ptype,
                                Total_sell_out = item.Total_sell_out,
                                Labels = item.Labels
                            };
                            newspaper_list.Add(temp_newspaper);
                        }
                    }

                }
                IEnumerable<Newspaper> result = newspaper_list;
                ViewData["kind"] = k;
                return View(result);
            }
        }

        private Newspaper Model_to_viewModel(Newspaper newspaper)
        {
            var editModel = new Newspaper();
            editModel.Pno_number = newspaper.Pno_number;
            editModel.Pna = newspaper.Pna;
            editModel.Ppr = newspaper.Ppr;
            editModel.Pdw = newspaper.Pdw;
            editModel.Ptype= newspaper.Ptype;
            editModel.Labels = newspaper.Labels;
            editModel.Img_url = newspaper.Img_url;
            return editModel;
        }
    }
}
