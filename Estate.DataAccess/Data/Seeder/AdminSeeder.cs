using Estate.Models;
using Microsoft.AspNetCore.Identity;
namespace Estate.DataAccess.Data.Seeder;
public class AdminSeeder
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AdminSeeder(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task SeedAdminAsync()
    {
        string adminEmail = "admin@example.com";
        string adminPassword = "Admin@123456"; 
        string adminRole = "Admin"; 

        
        var existingUser = await _userManager.FindByEmailAsync(adminEmail);
        if (existingUser != null)
        {
            
            return;
        }

        
        if (!await _roleManager.RoleExistsAsync(adminRole))
        {
            await _roleManager.CreateAsync(new IdentityRole(adminRole));
        }

        
        var user = new ApplicationUser
        {
            UserName = "09120000000",
            Email = adminEmail,
            EmailConfirmed = true,
            Name = "مدیر سیستم",
            Number = "09120000000",
            permisionDelete = true,
            permisionEdit = true,
            IsAgent = false,
            
        };

        
        var result = await _userManager.CreateAsync(user, adminPassword);

        if (result.Succeeded)
        {
            
            await _userManager.AddToRoleAsync(user, adminRole);
        }
        else
        {
            
            foreach (var error in result.Errors)
            {
                
            }
        }
    }
}
