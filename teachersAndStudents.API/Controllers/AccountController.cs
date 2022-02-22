using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using teachersAndStudents.API.Models;
using TeachersAndStudents.models;

namespace teachersAndStudents.API.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly IAccountModel account;

        public AccountController(IAccountModel account)
        {
            this.account = account;
        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> Register(SignUp model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = new IdentityUser
            {
                UserName = model.UserName,
                Email = model.Email,
            };
            var myuser = new User
            {
                FullName = model.FullName,
            };
            try
            {
                return Ok(await account.addUser(user, myuser, model.password,model.Role));
            }
            catch (System.Exception)
            {
                return BadRequest(new { massage = "server Error" });
            }
        }
        [HttpPost]
        public async Task<IActionResult> Login(Login model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                return Ok(await account.Login(model.UserName, model.password));
            }
            catch (System.Exception)
            {
                ModelState.AddModelError("massage", " البريد الإلكترونى أو كلمة السر غير صحيحة ");
                return BadRequest(ModelState);
            }
        }
        [HttpGet][Authorize]
        public async Task<IActionResult> check() { 
             if (!ModelState.IsValid)
                return BadRequest(ModelState);
             else
                return Ok(true);
        } 
    }
}
