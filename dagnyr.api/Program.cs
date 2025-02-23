using System.Text;
using dagnyr.api.Data;
using dagnyr.api.Entities;
using dagnyr.api.InterFaces;
using dagnyr.api.Repositories;
using dagnyr.api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var  serverVersion = new MySqlServerVersion(new Version(9, 2, 0));
builder.Services.AddDbContext<DataContext>
(options => {
    //options.UseSqlite(builder.Configuration.GetConnectionString("DevConnection"));
    //172.17.0.2
    //localhost
    options.UseMySql(builder.Configuration.GetConnectionString("MySQL"), serverVersion);
});

builder.Services.AddIdentityCore<User>(options =>
{
    options.User.RequireUniqueEmail = true;
    /*options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;*/
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<DataContext>();

builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<IOrderInformationRepository, OrderInformationRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddControllers();
        

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options=>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("tokenSettings:tokenKey").Value))
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

//if(app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<DataContext>();
    var userMgr = services.GetRequiredService<UserManager<User>>();
    var rolesMgr = services.GetRequiredService<RoleManager<IdentityRole>>();
    await context.Database.MigrateAsync();
    await Seed.LoadAddressTypes(context);
    await Seed.LoadCustomers(context);
    await Seed.LoadRoles(rolesMgr);
    await Seed.LoadUsers(userMgr);
    await Seed.LoadProducts(context);
    await Seed.LoadOrderInformations(context);
    await Seed.LoadOrderItems(context);
    
    

}
catch (Exception ex)
{
    System.Console.WriteLine("{0}", ex.Message);
    throw;
}
//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
