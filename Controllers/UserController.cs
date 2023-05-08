using TheWall.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Http;
using TheWall.Context;

namespace TheWall.Controllers;


public class UserController : Controller
{
    private TheWallContext _context;

    public UserController(TheWallContext context)
    {
        _context = context;
    }

    //////////? LoginReg View + Login and Reg Create Action ?///////////////////
    [HttpGet("")]
    public IActionResult LoginRegister()
    {
        int? userId = HttpContext.Session.GetInt32("userId");
        if (userId is not null)
        {
            return RedirectToAction("Messageboard", "Messageboard");
        }
        return View();
    }

    //////////! Login and Register Methods !/////////////////
    [HttpPost("users/create")]
    public IActionResult Register(User user)
    {
        if (ModelState.IsValid)
        {
            PasswordHasher<User> Hasher = new PasswordHasher<User>();
            user.Password = Hasher.HashPassword(user, user.Password);
            _context.Add(user);
            _context.SaveChanges();
            HttpContext.Session.SetInt32("userId", user.UserId);
            HttpContext.Session.SetString("firstName", user.FirstName);
            HttpContext.Session.SetString("lastName", user.LastName);
            return RedirectToAction("Messageboard", "MessageBoard");
        }
        else
        {
            return View("LoginRegister");
        }
    }

    //login method
    [HttpPost("users/login")]
    public IActionResult Login(Login user)
    {
        if (ModelState.IsValid)
        {

            User? userInDb = _context.Users.FirstOrDefault(u => u.Email == user.LEmail);

            if (userInDb == null)
            {
                ModelState.AddModelError("Email", "Invalid Email/Password");
                return View("LoginRegister");
            }

            PasswordHasher<Login> hasher = new PasswordHasher<Login>();

            var result = hasher.VerifyHashedPassword(user, userInDb.Password, user.LPassword);                                    // Result can be compared to 0 for failure        
            if (result == 0)
            {
                ModelState.AddModelError("Email", "Invalid Email/Password");
                return View("LoginRegister");
            }
            HttpContext.Session.SetInt32("userId", userInDb.UserId);
            HttpContext.Session.SetString("firstName", userInDb.FirstName);
            HttpContext.Session.SetString("lastName", userInDb.LastName);
            return RedirectToAction("Messageboard", "MessageBoard");
        }
        else
        {
            return View("LoginRegister");
        }
    }



    //////// * Logout Method * /////////////
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Remove("userId");
        HttpContext.Session.Remove("firstName");
        HttpContext.Session.Remove("lastName");
        return RedirectToAction("LoginRegister");
    }

}
