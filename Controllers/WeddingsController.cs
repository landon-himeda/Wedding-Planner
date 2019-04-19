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
    public class WeddingsController : Controller
    {
        private WeddingContext dbContext;
        public WeddingsController(WeddingContext context)
        {
            dbContext = context;
        }

        [HttpGet("Dashboard")]
        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetInt32("LoggedInUserId") is null)
            {
                return RedirectToAction("Index", "LoginReg");
            }

            User logged_in_user = dbContext.Users.FirstOrDefault(u => u.UserId == HttpContext.Session.GetInt32("LoggedInUserId"));
            List<Wedding> allWeddings = dbContext.Weddings.Include(w => w.RSVPs)
                .ThenInclude(r => r.user)
                .ToList();
            DashboardView viewModel = new DashboardView()
            {
                weddings = allWeddings,
                user = logged_in_user
            };
            return View(viewModel);
        }

        [HttpGet("delete/{weddingId}")]
        public IActionResult Delete(int weddingId)
        {
            if (HttpContext.Session.GetInt32("LoggedInUserId") is null)
            {
                return RedirectToAction("Index", "LoginReg");
            }

            Wedding weddingForDeletion = dbContext.Weddings.FirstOrDefault(w => w.WeddingId == weddingId);
            dbContext.Weddings.Remove(weddingForDeletion);
            dbContext.SaveChanges();

            return RedirectToAction("Dashboard");
        }

        [HttpGet("rsvp/{userId}/{weddingId}")]
        public IActionResult RSVP(int userId, int weddingId)
        {
            if (HttpContext.Session.GetInt32("LoggedInUserId") is null)
            {
                return RedirectToAction("Index", "LoginReg");
            }

            else if (userId != HttpContext.Session.GetInt32("LoggedInUserId"))
            {
                ModelState.AddModelError("User", "Cannot RSVP a different user");
                return View("Dashboard");
            }

            RSVP newRSVP = new RSVP()
            {
                UserId = userId,
                WeddingId = weddingId
            };

            dbContext.Add(newRSVP);
            dbContext.SaveChanges();

            return RedirectToAction("Dashboard");
        }

        [HttpGet("unrsvp/{userId}/{weddingId}")]
        public IActionResult UnRSVP(int userId, int weddingId)
        {
            if (HttpContext.Session.GetInt32("LoggedInUserId") is null)
            {
                return RedirectToAction("Index", "LoginReg");
            }

            else if (userId != HttpContext.Session.GetInt32("LoggedInUserId"))
            {
                ModelState.AddModelError("User", "Cannot Un-RSVP a different user");
                return View("Dashboard");
            }

            RSVP rsvpForDeletion = dbContext.RSVPs.Where(r => (r.UserId == userId) && (r.WeddingId == weddingId))
                                                .SingleOrDefault();
            dbContext.RSVPs.Remove(rsvpForDeletion);
            dbContext.SaveChanges();

            return RedirectToAction("Dashboard");
        }

        [HttpGet("new")]
        public IActionResult New()
        {
            if (HttpContext.Session.GetInt32("LoggedInUserId") is null)
            {
                return RedirectToAction("Index", "LoginReg");
            }
            return View();
        }

        [HttpPost("create")]
        public IActionResult Create(WeddingForm form)
        {
            if (HttpContext.Session.GetInt32("LoggedInUserId") is null)
            {
                return RedirectToAction("Index", "LoginReg");
            }

            if (ModelState.IsValid)
            {
                Wedding newWedding = new Wedding()
                {
                    WedderOne = form.WedderOne,
                    WedderTwo = form.WedderTwo,
                    Date = form.Date,
                    Address = form.Address.Replace(" ", "+"),
                    UserId = (int) HttpContext.Session.GetInt32("LoggedInUserId")
                };

                dbContext.Add(newWedding);
                dbContext.SaveChanges();

                int newWeddingId = dbContext.Weddings
                                        .Last(w => w.UserId == (int) HttpContext.Session.GetInt32("LoggedInUserId")).WeddingId;
                return RedirectToAction("ViewWedding", new { weddingId = newWeddingId });
            }
            return View("New");
        }

        [HttpGet("view/{weddingId}")]
        public IActionResult ViewWedding(int weddingId)
        {
            if (HttpContext.Session.GetInt32("LoggedInUserId") is null)
            {
                return RedirectToAction("Index", "LoginReg");
            }
            Wedding viewModel = dbContext.Weddings.Where(w => w.WeddingId == weddingId)
                                    .Include(w => w.RSVPs)
                                    .ThenInclude(r => r.user)
                                    .FirstOrDefault();
            return View(viewModel);
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "LoginReg");
        }

    }
}