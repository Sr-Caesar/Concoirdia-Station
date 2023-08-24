//using ConcordiaStation.Data.Services;
//using ConcordiaStation.WebApp.Models;
//using ConcordiaStation.WebApp.SecurityServices.Interfaces;
//using Microsoft.AspNetCore.Mvc;

//namespace ConcordiaStation.WebApp.Controllers
//{
//    public class LoginController : Controller
//    {
//        private readonly IServiceCredential _sCredential;
//        private readonly IServiceToken _sToken;

//        public LoginController(IServiceCredential serviceCredential, IServiceToken serviceToken)
//        {
//            _sCredential = serviceCredential;
//            _sToken = serviceToken;
//        }

//        [Route("login")]
//        public IActionResult Index()
//        {
//            var model = new CredentialModel();
//            return View(model);
//        }

//        [HttpPost]
//        public IActionResult LoginProcess(string email, string password)
//        {
//            var isEmailValid = _sCredential.CheckEmail(email);
//            var isPasswordValid = _sCredential.CheckPassword(password);
//            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
//            {
//                ModelState.AddModelError("", "Please enter both email and password.");
//                return View("Index");
//            }
//            else if (!isEmailValid)
//            {
//                ModelState.AddModelError("Email", "Invalid email. Please try again.");
//                return View("Index");
//            }
//            else if (!isPasswordValid)
//            {
//                ModelState.AddModelError("Password", "Invalid password. Please try again.");
//                return View("Index");
//            }
//            else
//            {
//                var id = _sCredential.GetIdByEmail(email);
//                var token = _sToken.GenerateToken(email, id);
//                var cookieOptions = new CookieOptions
//                {
//                    HttpOnly = true,
//                    Secure = false,
//                    SameSite = SameSiteMode.Strict
//                };
//                Response.Cookies.Append("token", token, cookieOptions);
//                return RedirectToAction("Index", "Home");
//            }
//        }

//        [HttpPost]
//        public IActionResult LogoutProcess()
//        {
//            string token = Request.Cookies["token"];
//            _sToken.AddExpiredToken(token, "blacklisttokens.json");
//            Response.Cookies.Delete("token");
//            return RedirectToAction("Index");
//        }
//    }
//}
