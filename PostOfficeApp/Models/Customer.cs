using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;


namespace PostOfficeApp.Models
{
    public class Customer
    {

        // 用户编码
        [Key]
        public int Gno { get; set; }
        // 用户等级 0 管理员 1 用户 创建只能创建用户，管理员是内置的

        //用户姓名
        public string Gna { set; get; }
        //gna varchar(20) not null,
        //用户电话 固定为11位 
        public string Gte { set; get; }
        //gte char (11) not null,
        //用户地址
        public string Gad { set; get; }
        //gad varchar(50),
        //邮政编码 固定为6位 
        public string Gpo { set; get; }
        //gpo char (6)
    };
    public class CustomerDbContext : DbContext
    {
        public CustomerDbContext(DbContextOptions<CustomerDbContext> options) : base(options)
        {
        }
        public DbSet<Customer> Customer { get; set; }
    }

}
