using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SchoolManagementConsuming.Models;
using System.Text;

namespace SchoolManagementConsuming.Controllers
{
    public class AuthController : Controller
    {
        HttpClient client;

        public AuthController()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            client = new HttpClient(clientHandler);
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(SignUp model)
        {
            string url = "https://localhost:7062/api/Auth/SignUp";
            var jsonData = JsonConvert.SerializeObject(model);
            var stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(url, stringContent).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("SignIn");
            }

            ViewBag.Message = "Registration failed.";
            return View(model);
        }

        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignIn model)
        {
            string url = "https://localhost:7062/api/Auth/signIn";
            var jsonData = JsonConvert.SerializeObject(model);
            var stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(url, stringContent);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<dynamic>(jsonResponse);

                HttpContext.Session.SetString("Username", (string)user.Username);
                HttpContext.Session.SetString("Role", (string)user.Role);

                return RedirectToAction("Index", "Event");
            }

            ViewBag.Message = "Login failed.";
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("SignIn");
        }
    }
}
