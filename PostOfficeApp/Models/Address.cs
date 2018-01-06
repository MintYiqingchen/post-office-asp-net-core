using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;


namespace PostOfficeApp.Models
{
    public class Address
    {
        [Required]
        public string Oaddress { get; set; }
        [Required]
        public string Gpo { get; set; }
        [Required]
        public string Gna { get; set; }
        [Required]
        [Phone]
        public string Gte { get; set; }
        public string Uid { get; set; }
    };
    public class AddressDbContext : DbContext
    {
        public AddressDbContext(DbContextOptions<AddressDbContext> options) : base(options)
        {
        }
        public DbSet<Address> Address { get; set; }
    }
    public class CanUpdate
    {
        public string Id { get; set; }
    }
    public class CanUpdateDbContext : DbContext
    {
        public CanUpdateDbContext(DbContextOptions<CanUpdateDbContext> options) : base(options)
        {
        }
        public DbSet<Address> CanUpdate { get; set; }
    }
}
