using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PostOfficeApp.Models
{
    public class Newspaper
    {
        //[Key]
        //public int Pno { get; set; }
        [Key]
        public string Pno_number { get; set; }
        public string Pna { get; set; }
        public float Ppr { get; set; }
        public string Pdw { get; set; }
        public string Ptype { get; set; }
        public int Total_sell_out { get; set; }
        public string Labels { get; set; }
        public string Img_url { get; set; }

    };
    public class NewspaperDbContext : DbContext
    {
        public NewspaperDbContext(DbContextOptions<NewspaperDbContext> options) : base(options)
        {
        }
        public DbSet<Newspaper> Newspaper { get; set; }
    }
}
