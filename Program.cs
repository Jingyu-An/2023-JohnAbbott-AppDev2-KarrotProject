using Karrot.Data;
using Karrot.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);
// var connectionString = builder.Configuration.GetConnectionString("KarrotDbContextConnection") ?? throw new InvalidOperationException("Connection string 'KarrotDbContextConnection' not found.");

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<KarrotDbContext>();
builder.Services.AddSession();

builder.Services.AddDefaultIdentity<IdentityUser>(opt =>
{
    opt.SignIn.RequireConfirmedAccount = false;
    opt.Password.RequireDigit = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<KarrotDbContext>()
.AddDefaultTokenProviders();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
