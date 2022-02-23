using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplicationAuthentication.Data;
using WebApplicationAuthentication.Models;

namespace WebApplicationAuthentication.Initializer
{
    public class DbInitalizer : IDbInitalizer
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitalizer(ApplicationDbContext db, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        
        public void Initalize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception e)
            {
                
            }

            if (_db.Roles.Any(r => r.Name == "Admin")) return;
            if (_db.Roles.Any(r => r.Name == "Staff")) return;

            _roleManager.CreateAsync(new IdentityRole("Admin")).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole("Staff")).GetAwaiter().GetResult();

            _userManager.CreateAsync(new ApplicationUser()
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                EmailConfirmed = true,
                FullName = "Admin",
                DateOfBirth = DateTime.Today,
                Address = "test street"
            }, "Admin123@").GetAwaiter().GetResult();

            ApplicationUser admin = _db.ApplicationUsers.Where(u => u.Email == "admin@gmail.com").FirstOrDefault();
            
            _userManager.AddToRoleAsync(admin, "Admin").GetAwaiter().GetResult();
        }
    }
}