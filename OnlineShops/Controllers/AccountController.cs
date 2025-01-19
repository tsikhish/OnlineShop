using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using OnlineShops.Models.DbModels;
using OnlineShops.Models.Services;

namespace OnlineShops.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userservice;
        public AccountController(IUserService userservice)
        {
            _userservice = userservice;
        }
        public async Task<IActionResult> RegisterUser()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterUser(UserRegistration user)
        {
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "User data is missing.");
                return View();
            }

            if (!ModelState.IsValid)
            {
                return View(user);
            }

            try
            {
                var newUser = await _userservice.Register(user);
                ViewData["Message"] = $"{newUser.UserName} registered successfully!";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(user);
            }
        }

        //public async Task<IActionResult> LoginUser([FromBody] LoginUser loginUser)
        //{
        //    try
        //    {
        //        var token = await _userservice.Login(loginUser);
        //        return Ok(token);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest($"{ex.Message}");
        //    }
        //}

    }

}