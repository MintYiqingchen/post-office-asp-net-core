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
        public int Onumber { get; set; }
        public int Ona { get; set; }
        public int Ofen { get; set; }
        public int Opeople { get; set; }
        public string Oaddress { get; set; }
        public int Ostart_year { get; set; }
        public float Oprice { get; set; }
        public int Last_time { get; set; }
        public int Pay_way { get; set; }
        public bool Boolpay { get; set; }
    }
    public class OrdersDbContext : DbContext
    {
        public OrdersDbContext(DbContextOptions<OrdersDbContext> options) : base(options)
        {
        }
        public DbSet<Orders> Orders { get; set; }
    }
}
