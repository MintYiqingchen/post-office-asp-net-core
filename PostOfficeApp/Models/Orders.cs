using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;


namespace PostOfficeApp.Models
{
    public class Orders
    {
        [Key]
        public string Onumber { get; set; }
        public string Ona { get; set; }//报纸名称

        [Required]
        public int Ofen { get; set; }
        public string Opeople { get; set; }
        
        // 配送信息
        [Required]
        public string Oaddress { get; set; }
        public string Gpo { get; set; }
        [Required]
        public string Gna { get; set; }//订购姓名
        [Required]
        [Phone]
        public string Gte { get; set; }

        public int Ostart_year { get; set; }
        public float Oprice { get; set; }
        public int Last_time { get; set; }
        public int Payway { get; set; }
        // 0:wei 1:yi wei 2:yi
        public int Boolpay { get; set; }
    }
    public class OrdersDbContext : DbContext
    {
        public OrdersDbContext(DbContextOptions<OrdersDbContext> options) : base(options)
        {
        }
        public DbSet<Orders> Orders { get; set; }
    }
    public class OrdersViewModel
    {
        public Newspaper Item1 { get; set; }
        public Orders Item2 { get; set; }
    }
}
