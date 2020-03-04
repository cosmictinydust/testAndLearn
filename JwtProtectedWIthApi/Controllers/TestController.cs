using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace JwtProtectedWIthApi.Controllers
{
    [Route("api/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {

        private readonly ILogger<TestController> _logger;
        private readonly IHttpContextAccessor _httpContext;

        public TestController(
            ILogger<TestController> logger,
            IHttpContextAccessor httpContext
            )
        {
            _logger = logger;
            _httpContext = httpContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            //创建声明数组 
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, "TestName"),
                new Claim(JwtRegisteredClaimNames.Email,"abc@qq.com"),
                new Claim(JwtRegisteredClaimNames.Sub,"1")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("abcdeabcdeabcdeabcdeabcde"));  
                //key的值应至少16位，太短的话会报错 IDX10603: Decryption failed. Keys tried: '[PII is hidden. For more details, see https://aka.ms/IdentityModel/PII.]'.

            //实例化一个新的token对象
            var token = new JwtSecurityToken(
                    issuer:"http://localhost:5000",
                    audience:"http://localhost:5001",
                    claims: claims,
                    expires:DateTime.Now.AddMinutes(2),
                    signingCredentials:new SigningCredentials(key,SecurityAlgorithms.HmacSha256)
                ); ;

            //生成Token
            var JwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return new string[] { JwtToken };
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> GetTokenFromContext()
        {
            var userNameFromControll = User.FindFirst(d => d.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;
            //"http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name" 这种是netcore 封装的Claim值，所以用Controll封装的User去提取Name值时，用的就是这个

            var userNameFromHttp = _httpContext.HttpContext.User.Identity.Name;
            
            var claims = _httpContext.HttpContext.User.Claims;
            var claimTypeVal = (from item in claims where item.Type == JwtRegisteredClaimNames.Email select item.Value).ToList();
            //注意，需要在Startup.cs中加入 JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); 因为JWT的自动映射Claim名要关闭。

            return new string[]
            {
                userNameFromControll,userNameFromHttp,JsonConvert.SerializeObject(claimTypeVal) };
        }

        [HttpGet]
        [Authorize]
        public ActionResult<string> GetSecret()
        {
            return "这是一个秘密";
        }
    }
}
