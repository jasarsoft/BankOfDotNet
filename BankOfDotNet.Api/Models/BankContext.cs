using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace BankOfDotNet.Api.Models
{
    public class BankContext : DbContext
    {
        public BankContext(DbContextOptions<DbContext> options) : base(options)
        { 
        }

        public DbSet<Customer> Customers { get; set; }
    }
}
