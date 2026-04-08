using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Zawdni.api.Data;     // الـ Context الخاص بقاعدة البيانات
using Zawdni.api.Models;
using Zawdni.Models.DTO;
using Zawdni.Services; // تأكد من استخدام الـ Namespace الصحيح لمجلد الـ Services

namespace Zawdni.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ZawdniDbContext _context; // افترضنا أن اسم الـ Context هو DataContext
        private readonly TokenService _tokenService;

        public AuthController(ZawdniDbContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginDTO loginDto)
        {
            // 1. البحث عن المستخدم في قاعدة البيانات
            var user = await _context.Users
                .SingleOrDefaultAsync(x => x.Name == loginDto.Username.ToLower());

            if (user == null) return Unauthorized("UnAuthorized User");


            var hasher = new PasswordHasher<User>();
            var result = hasher.VerifyHashedPassword(user, user.Password, loginDto.Password);
            // 2. التحقق من كلمة المرور (هنا نفترض مطابقة مباشرة، ويفضل لاحقاً استخدام Hash)
            if (result == PasswordVerificationResult.Failed) return Unauthorized("wrongpassorname");

            // 3. إذا كانت البيانات صحيحة، نولد التوكن
            var token = _tokenService.CreateToken(user);

            return Ok(new
            {
                username = user.Name,
                token = token
            });
        }
    }

    // كلاس بسيط لنقل البيانات القادمة من الـ Request
   
}