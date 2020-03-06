using JwtTest.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace JwtTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(
            ILogger<HomeController> logger,
            IHttpClientFactory httpClientFactory
            )
        {
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
            _httpClientFactory = httpClientFactory ?? throw new System.ArgumentNullException(nameof(httpClientFactory)); ;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult<string>> GetToken()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("http://localhost:5000/api/get?userName=user1&userPsw=password1");
            var result = response.Content.ReadAsStringAsync().Result;
            result = result.Replace("[", "").Replace("]", "").Trim(new char[1] { '"' });
            if (!string.IsNullOrWhiteSpace(result))
            {
                var options = new CookieOptions
                {
                    HttpOnly = true,
                    Domain = null,
                    IsEssential = true,
                    Expires = DateTime.Now.AddHours(1)
                };

                HttpContext.Response.Cookies.Append("co.save", result, options);
            }
            return result;
        }

        public async Task<ActionResult<string>> GetSecret()
        {
            var str = HttpContext.Request.Cookies["co.save"];
            if (string.IsNullOrWhiteSpace(str))
                return "未授权";
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", str);
            var response = await client.GetAsync("http://localhost:5000/api/GetSecret");
            var result = response.Content.ReadAsStringAsync().Result;
            return result;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
