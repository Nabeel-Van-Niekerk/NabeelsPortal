using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NabeelsPortal.Data;
using NabeelsPortal.DTO;

namespace NabeelsPortal.Controllers
{
    [Route("[controller]")]
    public class AccountController: Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AgriEnergyContext _context;
        public IActionResult Index()
        {
            return View();
        }

        public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, AgriEnergyContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        [HttpGet("Login")]
        public IActionResult Login()
        {
            return View();
        }



        [HttpPost("Login")]
        public async Task<IActionResult> Login(Login model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            IdentityUser user = null;

            if (model.EmailOrUsername.Contains("@"))
            {
                user = await _userManager.FindByEmailAsync(model.EmailOrUsername);
            }
            else
            {
                user = await _userManager.FindByNameAsync(model.EmailOrUsername);
            }

            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                // Check the role of the user
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Contains("Farmer"))
                {
                    return RedirectToAction("Index", "Farmer");
                }
                else if (roles.Contains("Employee"))
                {
                    return RedirectToAction("Index", "Employee");
                }
                else
                {
                    // Redirect to a default page if the user has no specific role
                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError("", "Invalid login attempt.");
            return View(model);
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}
