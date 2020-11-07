using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using KosmoForumClient.Models;
using KosmoForumClient.Models.View;
using KosmoForumClient.Repo.IRepo;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace KosmoForumClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAccountRepository _accountRepo;
        private readonly ICategoryRepository _categoryRepo;


        public HomeController(ILogger<HomeController> logger, IAccountRepository accountRepo, ICategoryRepository categoryRepo)
        {
            _logger = logger;
            _accountRepo = accountRepo;
            _categoryRepo = categoryRepo;
        }

        public async Task<IActionResult> Index()
        {
            var requestTuple = await _categoryRepo.GetAllAsync(SD.Categories, HttpContext.Session.GetString("JWToken"));
            if (requestTuple.Item1 != "")
            {
                TempData["error"] = requestTuple.Item1;
                return View(new CategoryVM() {CategoriesList = Enumerable.Empty<Category>()});
            }

            CategoryVM categoryVM = new CategoryVM
            {
                CategoriesList =
                    requestTuple.Item2
            };


            return View(categoryVM);
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

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            User obj = new User();
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string returnUrl, User userObj)
        {
            var obj = await _accountRepo.LoginAsync(SD.AccountApi + "authenticate", userObj);
            if (obj.Item2.Token == null)
            {
                TempData["error"] = obj.Item1;
                return View();
            }

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Name, userObj.Username));
            identity.AddClaim(new Claim(ClaimTypes.Role, obj.Item2.Role));

            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            HttpContext.Session.SetString("JWToken", obj.Item2.Token);
            TempData["alert"] = "Witaj" + obj.Item2.Username;
            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Register()
        {
            User obj = new User();
            return View(obj);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Register(User userObj)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    byte[] p1 = null;
                    using (var fs1 = files[0].OpenReadStream())
                    {
                        p1 = Resizer.Resize(fs1, 50, 50);
                    }

                    userObj.Avatar = p1;
                }

                Tuple<string,bool> result = await _accountRepo.RegisterAsync(SD.AccountApi+"register", userObj);
                if (result.Item2 == false)
                {
                    TempData["error"] = result.Item1;
                    return View();
                }

                TempData["alert"] = "Rejestracja zakończona sukcesem";
                return RedirectToAction("Login");
            }

            return View(userObj);
        }

        [Authorize]
        public async Task<IActionResult> Logout(User userObj)
        {
            await HttpContext.SignOutAsync();

            HttpContext.Session.SetString("JWToken","");
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
