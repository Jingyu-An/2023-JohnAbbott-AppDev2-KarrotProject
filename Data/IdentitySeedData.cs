using Microsoft.AspNetCore.Identity;

namespace Karrot.Data;

public class IdentitySeedData
{
    public static async Task Initialize(UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        //context.Database.EnsureCreated();

        string adminRole = "Admin";
        string userRole = "User";

        if (await roleManager.FindByNameAsync(adminRole) == null)
        {
            await roleManager.CreateAsync(new IdentityRole(adminRole));
        }

        if (await roleManager.FindByNameAsync(userRole) == null)
        {
            await roleManager.CreateAsync(new IdentityRole(userRole));
        }
    }
}