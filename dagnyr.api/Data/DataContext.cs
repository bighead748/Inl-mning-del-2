using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using dagnyr.api.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace dagnyr.api.Data;

public class DataContext(DbContextOptions options) : IdentityDbContext<User>(options)
{
    public DbSet<Product> Products { get; set; }
    public DbSet<OrderInformation> OrderInformations { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<PostalAddress> PostalAddresses { get; set; }
    public DbSet<AddressType> AddressTypes { get; set; }
    public DbSet<CustomerAddress> CustomerAddresses { get; set; }
    
    
    
    
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OrderItem>().HasKey(o => new { o.OrderInformationId, o.ProductId });
        modelBuilder.Entity<CustomerAddress>().HasKey(c => new { c.AddressId, c.CustomerId });
        
       
        
        
        
        base.OnModelCreating(modelBuilder);
    }
}
