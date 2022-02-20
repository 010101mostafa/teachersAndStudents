using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using teachersAndStudents.API.Modules;
using TeachersAndStudents.models;

namespace teachersAndStudents.API.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly IAccountModule account;

        public AccountController( IAccountModule account)
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
            if (model.Role == ERole.Student)
            {
                var student = new Student
                {
                    UserId = user.Id,
                    FullName = model.FullName,
                };
                try
                {
                    return Ok(await account.addStudent(user,student,model.password));
                }
                catch (System.Exception)
                {
                    return BadRequest(new { massage = "server Error" });
                }
            }
            else if (model.Role == ERole.Teacher) {
                var teacher = new Teacher
                {
                    UserId = user.Id,
                    FullName = model.FullName,
                };
                try
                {
                    return Ok(await account.addTeacher(user, teacher, model.password));
                }
                catch (System.Exception)
                {
                    return BadRequest(new { massage = "server Error" });
                }
            }
            else
                return BadRequest(new { massage = "select a valid role" });
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
