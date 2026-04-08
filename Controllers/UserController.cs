using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        // GET: api/<UserController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UserController>
        [HttpPost("NewUser")]
        public IActionResult NewUser([FromBody] UserDTO userDTO)
        {
            var pass = new PasswordHasher<User>();

            var User = new User
            { Name = userDTO.Name,
             Password = pass.HashPassword(null,userDTO.Password),
              Email = userDTO.Email 
            };
            _dbcontext.Users.Add(User);
            _dbcontext.SaveChanges();
            return Ok("user"+" "+User.Name+" "+"has been added");
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
