using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using WebAppProfile.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebAppProfile.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppIdentityDbContext _identityDbContext;
        private readonly IWebHostEnvironment _hosting;

        public UserController(UserManager<AppUser> userManager, AppIdentityDbContext identityDbContext, IWebHostEnvironment hosting)
        {
            _userManager = userManager;
            _identityDbContext = identityDbContext;
            _hosting = hosting;
        }
        [Authorize]
        public IActionResult Index()
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            var usernameLogin = _userManager.GetUserName(currentUser); //username dari siapa yg login

            var data = _identityDbContext.AppUsers.Where(e => e.UserName == usernameLogin).SingleOrDefault();

            ViewData["username"] = data.UserName;
            ViewData["name"] = data.Fullname;
            return View();
        }
        [Authorize]
        [Route("user/{username}")]
        public IActionResult Profile(string username)
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            var usernameLogin = _userManager.GetUserName(currentUser); //username dari siapa yg login
            
            var data = _identityDbContext.AppUsers.Where(e => e.UserName == username ).SingleOrDefault();
            
            if (usernameLogin == username) 
            {
                //ViewBag.username = username;
                return View(data);
            }

            return View("ProfilePublic", data);
        }
        [Authorize]
        public IActionResult Edit() 
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditAsync(AppUser usr)
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            var idLogin = _userManager.GetUserId(currentUser); //username dari siapa yg login
            string uniqueFileName = UploadImage(usr.ImagePath);

            var data = await _userManager.FindByIdAsync(idLogin);

            data.Fullname = usr.Fullname;
            data.Email = usr.Email;
            data.PhoneNumber = usr.PhoneNumber;
            data.Address = usr.Address;
            data.City = usr.City;
            data.Path = uniqueFileName;

            var result = await _userManager.UpdateAsync(data);
            return RedirectToAction("Index","User");

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
    }
}
