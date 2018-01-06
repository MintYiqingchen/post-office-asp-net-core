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
        public string Ona { get; set; }
        public int Ofen { get; set; }
        public int Opeople { get; set; }
        
        // 配送信息
        public string Oaddress { get; set; }
        [StringLength(maximumLength:6,ErrorMessage ="请输入正确的邮政编码",MinimumLength =6)]
        public string Gpo { get; set; }
        [Required]
        public string Gna { get; set; }
        [Phone(ErrorMessage ="请输入有效联系电话")]
        public string Gte { get; set; }

        
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
