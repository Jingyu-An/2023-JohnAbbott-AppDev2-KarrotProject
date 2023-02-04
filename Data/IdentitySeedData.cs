using Microsoft.AspNetCore.Identity;

namespace Karrot.Data;

public class IdentitySeedData
{
    public static async Task Initialize(UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {

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

        string admin = "Admin";
        string adminEmail = "admin@admin.com";
        string adminPass = "Admin123!";
        if (userManager.FindByNameAsync(admin).Result == null)
        {
            IdentityUser user = new IdentityUser();
            user.UserName = admin;
            user.Email = adminEmail;
            user.PhoneNumber = "7777777777";
            IdentityResult result = userManager.CreateAsync(user, adminPass).Result;

            if (result.Succeeded)
            {
                var result2 = userManager.AddToRoleAsync(user, "Admin").Result;
                // if (!result2.Succeeded)
                // FIXME: log the error
            }
        }
    }
}