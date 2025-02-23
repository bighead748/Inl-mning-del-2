using System.Text.Json;
using dagnyr.api.Entities;
using dagnyr.api.ViewModels.Customer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace dagnyr.api.Data;

public static class Seed
{

    private static readonly JsonSerializerOptions options = new ()
    {
        PropertyNameCaseInsensitive = true
    };

    

    public static async Task LoadProducts(DataContext context) 
        {
            if(context.Products.Any()) return;

            var json = File.ReadAllText("Data/json/products.json");
            var products = JsonSerializer.Deserialize<List<Product>>(json, options);

            if(products is not null && products.Count > 0)
            {
                await context.Products.AddRangeAsync(products);
                await context.SaveChangesAsync();
            }
        }

        public static async Task LoadOrderInformations(DataContext context) 
        {
            if (context.OrderInformations.Any()) return;

            var json = File.ReadAllText("Data/json/orderinformations.json");
            var orders = JsonSerializer.Deserialize<List<OrderInformation>>(json, options);

            if (orders is not null && orders.Count > 0)
            {
                foreach (var order in orders)
                {
                    
                    var customerExists = await context.Customers.AnyAsync(c => c.Id == order.CustomerId);
                    if (!customerExists)
                    {
                        Console.WriteLine($"Skipping order {order.OrderNumber}: CustomerId {order.CustomerId} not found.");
                        continue; 
                    }

                    await context.OrderInformations.AddAsync(order);
                }
                
                await context.SaveChangesAsync();
            }
        }

        public static async Task LoadOrderItems(DataContext context)
        {
            if (context.OrderItems.Any()) return;

            var json = File.ReadAllText("Data/json/orderitems.json");
            var orderitems = JsonSerializer.Deserialize<List<OrderItem>>(json, options);

            if (orderitems is not null && orderitems.Count > 0)
            {
            await context.OrderItems.AddRangeAsync(orderitems);
            await context.SaveChangesAsync();
            }
        }

        
        

        public static async Task LoadAddressTypes(DataContext context)
        {
            if (context.AddressTypes.Any()) return;

            var json = await File.ReadAllTextAsync("Data/json/addressTypes.json");
            var types = JsonSerializer.Deserialize<List<AddressType>>(json, options);

            if (types is not null && types.Count > 0)
            {
            await context.AddressTypes.AddRangeAsync(types);
            await context.SaveChangesAsync();
            }
        }

        

        public static async Task LoadRoles(RoleManager<IdentityRole> roleManager)
        {
            if(roleManager.Roles.Any()) return;

            var admin = new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" };
            var user = new IdentityRole { Name = "User", NormalizedName = "USER" };
            var sales = new IdentityRole { Name = "SalesSupport", NormalizedName = "SALESSUPPORT" };

            await roleManager.CreateAsync(admin);
            await roleManager.CreateAsync(user);
            await roleManager.CreateAsync(sales);
        }

        public static  async Task LoadUsers(UserManager<User> userManager)
        {
            if(userManager.Users.Any()) return;

            var evert = new  User
            {
                FirstName ="Evert", 
                LastName = "Andersson", 
                UserName = "evert@gmail.com", 
                Email = "evert@gmail.com"
            };

            await userManager.CreateAsync(evert, "Password01!");
            await userManager.AddToRoleAsync(evert, "User");

            var helena = new  User
            {
                FirstName ="Helena", 
                LastName = "Eriksson", 
                UserName = "helena@gmail.com", 
                Email = "helena@gmail.com"
            };

            await userManager.CreateAsync(helena, "Password01!");
            await userManager.AddToRolesAsync(helena, ["User", "Admin", "SalesSupport"]);
            


        }

    public static async Task LoadCustomers(DataContext context) 
    {

        if(context.Customers.Any()) return;

        var json = File.ReadAllText("Data/json/customers.json");
        var customers = JsonSerializer.Deserialize<List<PostCustomerViewModel>>(json, options);

        if(customers is not null && customers.Count > 0)
        {
                foreach (var customer in customers)
                {
                    var newCustomer = new Customer
                    {
                        StoreName = customer.StoreName,
                        ContactPerson = customer.ContactPerson,
                        Email = customer.Email,
                        Phone = customer.Phone
                    };

                    await context.Customers.AddAsync(newCustomer);

                    foreach (var a in customer.Addresses)
                    {
                        var postalAddress = await context.PostalAddresses.FirstOrDefaultAsync(c => c.PostalCode.Replace(" ", "").Trim() == a.PostalCode.Replace(" ", "").Trim());
                        var address = await context.Addresses.FirstOrDefaultAsync(c => c.AddressLine.Trim().ToLower() == a.AddressLine.Trim().ToLower());

                        if (postalAddress == null)
                        {
                            postalAddress = new PostalAddress { PostalCode = a.PostalCode.Replace(" ", "").Trim(), City = a.City.Trim() };
                            await context.PostalAddresses.AddAsync(postalAddress);
                        }

                        if (address == null)
                        {
                            address = new Address { AddressLine = a.AddressLine, AddressTypeId = (int)a.AddressType, PostalAddress = postalAddress };
                            await context.Addresses.AddAsync(address);
                        }

                        if(newCustomer.CustomerAddresses == null)
                        {
                            newCustomer.CustomerAddresses = new List<CustomerAddress>();
                        }
                        newCustomer.CustomerAddresses.Add(new CustomerAddress { Address = address, Customer = newCustomer });
                    }

                    await context.SaveChangesAsync();
                }
            

                
        }
    }

}

