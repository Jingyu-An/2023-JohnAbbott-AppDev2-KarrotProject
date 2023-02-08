using Karrot.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Karrot.Areas.Identity.Pages.Account.Manage.Users;

public class LockOut : PageModel
{
    private readonly KarrotDbContext context;
    private readonly ILogger<LockOut> logger;
    private readonly SignInManager<IdentityUser> signInManager;
    private readonly UserManager<IdentityUser> userManager;

    public LockOut(KarrotDbContext context,
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        ILogger<LockOut> logger)
    {
        this.context = context;
        this.logger = logger;
        this.userManager = userManager;
        this.signInManager = signInManager;
    }

    [BindProperty] public IdentityUser User { get; set; }

    public async Task<IActionResult> OnGetAsync(string? id)
    {
        if (id == null || context.Users == null)
        {
            return NotFound();
        }

        var user = await context.Users.FindAsync(id);

        if (user == null)
        {
            return NotFound();
        }
        else
        {
            User = user;
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(string? id)
    {
        if (id == null || context.Users == null)
        {
            return NotFound();
        }

        var user = await context.Users.FindAsync(id);

        if (user != null)
        {
            user.LockoutEnd = User.LockoutEnd;
            var result = await userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                logger.LogInformation($"{user.UserName} is deleted");
            }
        }

        return RedirectToPage("../Users");
    }
}