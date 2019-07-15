using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VOD.UI.Models;
using Microsoft.AspNetCore.Identity;
using VOD.Common.Entities;
using VOD.Database.Services;

namespace VOD.UI.Controllers
{
    public class HomeController : Controller
    {
        //private IUIReadService _db;
        private SignInManager<VODUser> _signInManager;

        public HomeController(SignInManager<VODUser> signInManager)
        {
            _signInManager = signInManager;
            //_db = db;
        }
        public IActionResult Index()
        {
            //var courses = (await _db.GetCourses("14e4b425-ff96-4ef3-834a-ee2e496105c5")).ToList();
            //var course = await _db.GetCourse("14e4b425-ff96-4ef3-834a-ee2e496105c5", 2);
            if (!_signInManager.IsSignedIn(User))
            {
                return RedirectToPage("/Account/Login", new { Area="Identity"});
            }
            return RedirectToAction("Dashboard", "Membership");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
