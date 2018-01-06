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

namespace PostOfficeApp.Controllers
{   
    public enum KIND
    {
        NEWSPAPER,MAGZINE,ALL
    };

    public class HomeController : Controller
    {
        private readonly MovieDbContext _context;
        private readonly CustomerDbContext _context1;
        private readonly NewspaperDbContext _context2;
        private readonly OrdersDbContext _context3;
        public HomeController(MovieDbContext context, CustomerDbContext context1, NewspaperDbContext context2, OrdersDbContext context3)
        {
            _context = context;
            _context1 = context1;
            _context2 = context2;
            _context3 = context3;

        }

        public HomeController(MovieDbContext context)
        {
            _context = context;
        }
        // http get
        public async Task<IActionResult> Index()
        {
            /**/
            
            return View(await _context.Movie.ToListAsync());
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
        public JsonResult Search(string q="",KIND k = KIND.ALL)
        {
            /*@usage: 返回走索栏需要的json结果
             * @q:搜索关键词
             * @k:见KIND定义
             */
            JObject returnContent = new JObject();
            IEnumerable<string> keys = q.ToLower().Split();
            foreach (var s in keys)
            {
                if (Regex.IsMatch(s, @"^[0-9]+-[0-9]+$"))
                {
                    //根据报刊发行号搜索
                    var query = from rows in _context2.Newspaper
                                where rows.Pno_number == q
                                select new
                                {
                                    rows.Pno,
                                    rows.Pno_number,
                                    rows.Pna,
                                    rows.Ppr,
                                    rows.Pdw,
                                    rows.Ptype,
                                    rows.Total_sell_out,
                                    rows.Labels
                                };

                    List<Newspaper> newspaper_list = new List<Newspaper>();
                    foreach (var item in query)
                    {
                        Newspaper temp_newspaper = new Newspaper
                        {
                            Pno = item.Pno,
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
                                new JObject(new JProperty("title", newspaper_list[0].Pna), new JProperty("url", "/details/" + newspaper_list[0].Pno))
                            )
                        ));
                    }
                    else
                    {
                        returnContent = new JObject(
                        new JProperty("results",
                            new JArray(
                                //返回的url为/test/pno
                                new JObject(new JProperty("title", "Not Found"), new JProperty("url", "/test/"))
                            )
                        ));
                    }
                    return new JsonResult(returnContent);
                }
                else
                {
                    //根据报刊名搜索
                    var query = from rows in _context2.Newspaper
                                where rows.Pna == q
                                select new
                                {
                                    rows.Pno,
                                    rows.Pna,
                                    rows.Ppr,
                                    rows.Pdw,
                                    rows.Ptype,
                                    rows.Total_sell_out,
                                    rows.Labels
                                };

                    List<Newspaper> newspaper_list = new List<Newspaper>();
                    foreach (var item in query)
                    {
                        Newspaper temp_newspaper = new Newspaper
                        {
                            Pno = item.Pno,
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
                                   new JObject(new JProperty("title", newspaper_list[0].Pna), new JProperty("url", "/details/" + newspaper_list[0].Pno))
                               )
                           ));
                    }
                    else
                    {
                        returnContent = new JObject(
                           new JProperty("results",
                               new JArray(
                                   new JObject(new JProperty("title", "Not Found"), new JProperty("url", "/test/error"))
                               )
                           ));
                    }
                    break;
                }
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
            IEnumerable<string> keys = q.ToLower().Split(',');
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
                                    rows.Pno,
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
                            Pno = item.Pno,
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
                                         rows.Pno,
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
                                Pno = item.Pno,
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
                                         rows.Pno,
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
                                Pno = item.Pno,
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

    }
}
