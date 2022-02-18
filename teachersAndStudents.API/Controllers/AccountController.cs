using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using teachersAndStudents.API.Entitys;
using teachersAndStudents.API.Modules;

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

        [HttpPost("SignIn")]
        public async Task<IActionResult> Register(Signin model)
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
                    return Ok(account.addStudent(user,student,model.password));
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
                    return Ok(account.addTeacher(user, teacher, model.password));
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
                return Ok(account.Login(model.UserName, model.password));
            }
            catch (System.Exception)
            {
                ModelState.AddModelError("massage", " البريد الإلكترونى أو كلمة السر غير صحيحة ");
                return BadRequest(ModelState);
            }
        }
    }
}
