using Microsoft.AspNetCore.Mvc;
using OrderPortal.Data;
using OrderPortal.Models;

namespace OrderPortal.Controllers
{
    public class AccountController : Controller
    {
        private readonly StoreRepository _repository;

        public AccountController(StoreRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Any username and password are accepted for now
                var login = _repository.GetLoginByEmail(model.Email);
                
                // Store login info in session
                HttpContext.Session.SetInt32("LoginPK", login!.LoginPK);
                HttpContext.Session.SetInt32("CustomerId", login!.CustomerId);
                HttpContext.Session.SetString("UserEmail", login!.Email);
                
                return RedirectToAction("Index", "Store");
            }
            
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
