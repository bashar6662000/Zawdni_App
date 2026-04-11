using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Security.Claims;
using Zawdni.api.Data;
using Zawdni.api.Models;
using Zawdni.Models.DTO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Zawdni.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ZawdniDbContext _dbcontext;
        public UserController (ZawdniDbContext zawdniDbContext)
        {
            _dbcontext = zawdniDbContext;
        }
        
       


        // POST api/<UserController>
        [HttpPost("NewUser")]
        public IActionResult NewUser([FromBody] UserDTO userDTO)
        {
            var pass = new PasswordHasher<User>();

            var User = new User

            {
                 Name = userDTO.Name,
                 Password = pass.HashPassword(null,userDTO.Password),
                 Email = userDTO.Email 
            };
            _dbcontext.Users.Add(User);
            _dbcontext.SaveChanges();
            return Ok("user"+" "+User.Name+" "+"has been added");
        }
        [HttpPut("EditInfo")]
        public IActionResult EditInfo([FromBody] UserDTO userDTO)
        {
            
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
           
            if (userId == null)
                return BadRequest("Please Login first");
            if(int.Parse(userId)!=userDTO.Id)
                return NotFound("NIGGA THIS NOT UR DATA U CANT EDIT IT");

            var UserInDb = _dbcontext.Users.Find(int.Parse(userId));

            if (UserInDb == null)
                return Ok("Nothing Changed ");

            UserInDb.Name=userDTO.Name;
            UserInDb.Email=userDTO.Email;
            UserInDb.PhoneNumber=userDTO.PhoneNumber;
            UserInDb.ClinicName=userDTO.ClinicName;
            UserInDb.ClinicAddress=userDTO.ClinicAddress;

            return Ok(" new Info has been applied");

        }
        
    
    }
}
