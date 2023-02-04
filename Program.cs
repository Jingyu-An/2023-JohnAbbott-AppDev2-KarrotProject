using Karrot.Data;
using Karrot.Hub;
using Karrot.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);
    
//var config = builder.Configuration; //For external provider authentication

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSignalR();
builder.Services.AddDbContext<KarrotDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("KarrotDbContext")));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddSession();

builder.Services.AddDefaultIdentity<IdentityUser>(opt =>
{
    opt.SignIn.RequireConfirmedAccount = false;
    opt.SignIn.RequireConfirmedPhoneNumber = false;
    
    opt.Password.RequireDigit = false;
    opt.Password.RequireLowercase = false;
    opt.Password.RequireUppercase = false;
    opt.Password.RequireNonAlphanumeric = false;
    opt.Password.RequiredLength = 4;
    
    // Lockout settings.
    opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    opt.Lockout.MaxFailedAccessAttempts = 5;
    opt.Lockout.AllowedForNewUsers = true;
    
    // User settings.
    opt.User.AllowedUserNameCharacters =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_";
    opt.User.RequireUniqueEmail = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<KarrotDbContext>()
.AddDefaultTokenProviders();


// TODO: For Google provider authentication
// builder.Services.AddAuthentication()
//     .AddGoogle(options =>
//     {
//         IConfigurationSection googleAuthNSection =
//             config.GetSection("Authentication:Google");
//         options.ClientId = googleAuthNSection["ClientId"];
//         options.ClientSecret = googleAuthNSection["ClientSecret"];
//     });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<KarrotDbContext>();
    //context.Database.Migrate();
    // context.Database.EnsureCreated();

    var userMgr = services.GetRequiredService<UserManager<IdentityUser>>();  
    var roleMgr = services.GetRequiredService<RoleManager<IdentityRole>>();  

    IdentitySeedData.Initialize(userMgr, roleMgr).Wait();
    DbInitializer.Initialize(context);
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.MapHub<ChatHub>("chatHub");

app.Run();



