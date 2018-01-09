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
        [Key]
        [Required]
        public string Pno_number { get; set; }
        [Required]
        public string Pna { get; set; }
        [Required]
        [Range(0,9999)]
        public float Ppr { get; set; }
        public string Pdw { get; set; }
        [Required]
        public string Ptype { get; set; }
        public int Total_sell_out { get; set; }
        [Required]
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
