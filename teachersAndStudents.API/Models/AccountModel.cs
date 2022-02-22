using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using teachersAndStudents.API.Services;
using TeachersAndStudents.models;

namespace teachersAndStudents.API.Models
{
    public interface IAccountModel
    {
        public Task<string> addUser(IdentityUser user, User myuser, string Password, ERole role);
        public Task<string> Login(string UserName, string Password);
    }
    public class AccountModel: IAccountModel
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IAuthService authService;
        private readonly AppDbContext appDbContext;

        public AccountModel(UserManager<IdentityUser> userManager, IAuthService authService, AppDbContext appDbContext) {
            this.userManager = userManager;
            this.authService = authService;
            this.appDbContext = appDbContext;
        }
        public async Task<string> addUser(IdentityUser user, User myuser, string Password,ERole role) {
            await authService.addRoles();
            var result = await userManager.CreateAsync(user, Password);
            if (!result.Succeeded)
            {
                var errors = "";
                foreach (var e in result.Errors)
                    errors += " " + e.Description;
                throw new Exception(errors);
            }
            myuser.UserId= (await userManager.FindByNameAsync(user.UserName)).Id;
            if (role == ERole.Student)
            {
                await appDbContext.Students.AddAsync(new Student { 
                    FullName = myuser.FullName,
                    UserId=myuser.UserId,
                });
            }
            else if (role == ERole.Teacher)
            {
                await appDbContext.Teachers.AddAsync(new Teacher
                {
                    FullName = myuser.FullName,
                    UserId = myuser.UserId,
                });
                await userManager.AddToRoleAsync(user, "Teacher");
            }
            await userManager.AddToRoleAsync(user, "Student");
            await appDbContext.SaveChangesAsync();
            return await authService.CreateJwtToken(user);
        }
        public async Task<string> Login(string UserName, string Password){
            var user = await userManager.FindByNameAsync(UserName);
            if (user==null || !await userManager.CheckPasswordAsync(user, Password))
            {
                throw new System.Exception("the username or password is incorrect");
            }
            return await authService.CreateJwtToken(user);
            }
    }
}
