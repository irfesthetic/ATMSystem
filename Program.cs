using ATMSystem.Data;
using ATMSystem.Models;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSession();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.UseSession();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    if (!db.Customers.Any(c => c.AccountNumber == "100001"))
    {
        db.Customers.Add(new Customer
        {
            FullName = "Demo User",
            AccountNumber = "100001",
            Pin = "1234",
            Balance = 25000,
            IsActive = true
        });
    }

    if (!db.Customers.Any(c => c.AccountNumber == "100002"))
    {
        db.Customers.Add(new Customer
        {
            FullName = "Test Receiver",
            AccountNumber = "100002",
            Pin = "5678",
            Balance = 10000,
            IsActive = true
        });
    }

    db.SaveChanges();
    app.Run();
}
