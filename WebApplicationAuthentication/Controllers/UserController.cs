using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplicationAuthentication.Data;
using WebApplicationAuthentication.Models;

namespace WebApplicationAuthentication.Controllers
{
    [Authorize(Roles = "Admin,Staff")]
    public class UserController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;

        public UserController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // take id of current login user
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var userList = _db.ApplicationUsers.Where(u => u.Id != claims.Value).ToList();

            foreach (var user in userList)
            {
                var userTemp = await _userManager.FindByIdAsync(user.Id);
                var roleTemp = await _userManager.GetRolesAsync(userTemp);
                user.Role = roleTemp.First();
            }

            if (User.IsInRole("Staff"))
            {
                return View(userList.Where(u => u.Role != "Admin").ToList());
            }

            return View(userList);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            await _userManager.DeleteAsync(user);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Update(string id)
        {
            if (id!= null)
            {
                var user = _db.ApplicationUsers.Find(id);
                return View(user);
            }

            return NotFound();
        }

        [HttpPost]
        public IActionResult Update(ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
                var userInDb = _db.ApplicationUsers.Find(applicationUser.Id);
                userInDb.FullName = applicationUser.FullName;
                userInDb.Address = applicationUser.Address;
                userInDb.DateOfBirth = applicationUser.DateOfBirth;
                userInDb.PhoneNumber = applicationUser.PhoneNumber;

                _db.ApplicationUsers.Update(userInDb);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(applicationUser);
        }
    }
}