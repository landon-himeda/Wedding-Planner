using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WeddingPlanner.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace WeddingPlanner.Controllers
{
    public class LoginRegController : Controller
    {
        private WeddingContext dbContext;

        public LoginRegController(WeddingContext context)
        {
            dbContext = context;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("register")]
        public IActionResult Register(RegUser form)
        {
            if(ModelState.IsValid)
            {
                if(dbContext.Users.Any(u => u.Email == form.Email))
                {
                    ModelState.AddModelError("Email", "Email already in use");
                    return View("Index");
                }

                User newUser = new User()
                {
                    FirstName = form.FirstName,
                    LastName = form.LastName,
                    Username = form.Username,
                    Email = form.Email,
                };
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                newUser.Password = Hasher.HashPassword(newUser, form.Password);

                dbContext.Add(newUser);
                dbContext.SaveChanges();

                // Log user into session
                HttpContext.Session.SetInt32("LoggedInUserId", newUser.UserId);

                return RedirectToAction("Dashboard", "Weddings");
            }
            return View("Index");
        }

        [HttpPost("login")]
        public IActionResult Login(LogUser form)
        {
            if(ModelState.IsValid)
            {
                // If inital ModelState is valid, query for a user with provided email
                var userInDb = dbContext.Users.FirstOrDefault(u => u.Email == form.LogEmail);
                // If no user exists with provided email
                if(userInDb == null)
                {
                    // Add an error to ModelState and return to View!
                    ModelState.AddModelError("LogEmail", "Invalid Email/Password");
                    return View("Index");
                }
                
                // Initialize hasher object
                var hasher = new PasswordHasher<LogUser>();
                
                // verify provided password against hash stored in db
                var result = hasher.VerifyHashedPassword(form, userInDb.Password, form.LogPassword);
                
                // result can be compared to 0 for failure
                if(result == 0)
                {
                    ModelState.AddModelError("LogEmail", "Invalid Email/Password");
                    return View("Index");
                }

                // Log user into session
                HttpContext.Session.SetInt32("LoggedInUserId", userInDb.UserId);

                return RedirectToAction("Dashboard", "Weddings");
            }
            return View("Index");
        }
    }
}
