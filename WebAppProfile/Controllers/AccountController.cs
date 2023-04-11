using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebAppMvcSecureDB.Models;
using WebAppProfile.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebAppProfile.Controllers
{
    public class AccountController : Controller
    {
        // DI
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IWebHostEnvironment _hosting;
        private readonly AppIdentityDbContext _context;

        public AccountController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager, IWebHostEnvironment hosting, AppIdentityDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _hosting = hosting;
            _context = context;
        }
        //generate roles
        //public IActionResult GenerateRoles()
        //{
        //    string[] roles = { "Admin", "Manager", "Member" };
        //    foreach (var role in roles)
        //    {
        //        var hasil = _roleManager.CreateAsync(new IdentityRole(role)).GetAwaiter().GetResult();
        //    }
        //    return View("Index");
        //}
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RegisterAsync(RegisterView usr)
        {
            // validasi
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadImage(usr.ImagePath);
                AppUser user = new AppUser
                {
                    UserName = usr.Username,
                    Fullname = usr.Fullname,
                    Email = usr.Email,
                    PhoneNumber = usr.PhoneNumber,
                    Address = usr.Address,
                    City = usr.City,
                    Path = uniqueFileName,
                    PasswordHash = usr.Password

                };
                var result = await _userManager.CreateAsync(user, usr.Password);
                return RedirectToAction("Login", "Account" );
            }
            return View();
            
        }
        private string UploadImage(IFormFile file)
        {
            string uniqueFileName = string.Empty;
            if (file != null)
            {
                string uploadFolder = _hosting.WebRootPath + "/upload/";
                uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                string filePath = Path.Combine(uploadFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }
        public IActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginView usr, string? returnUrl)
        {
            //  if (ModelState.IsValid)
            //  {
            var usrapp = _userManager.FindByNameAsync(usr.UserName).GetAwaiter().GetResult();
            if (usrapp != null)
            {
                // session clear
                _signInManager.SignOutAsync().GetAwaiter().GetResult();
                var hasil = _signInManager.PasswordSignInAsync(usrapp, usr.Password, false, false).GetAwaiter().GetResult();
                if (hasil.Succeeded) return Redirect(returnUrl ?? "/User/" + usr.UserName);
            }
            //  }
            return View();
        }
        public IActionResult Logout()
        {
            _signInManager.SignOutAsync().GetAwaiter().GetResult();
            return Redirect("/home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
