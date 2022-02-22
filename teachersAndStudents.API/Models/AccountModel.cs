using Microsoft.AspNetCore.Identity;
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
                throw new System.Exception(result.Errors.ToString());
            }
            myuser.UserId= (await userManager.FindByNameAsync(user.UserName)).Id;
            await userManager.AddToRoleAsync(user, "Student");
            if (role == ERole.Student)
            {
                await appDbContext.Students.AddAsync((Student)myuser);
            }
            else if (role == ERole.Teacher)
            {
                await appDbContext.Teachers.AddAsync((Teacher)myuser);
                await userManager.AddToRoleAsync(user, "Teacher");
            }
            await appDbContext.SaveChangesAsync();
            return await authService.CreateJwtToken(user);
        }
        public async Task<string> Login(string UserName, string Password){
            var user = await userManager.FindByNameAsync(UserName);
            if (user==null || !await userManager.CheckPasswordAsync(user, Password))
            {
                return null;
            }
            return await authService.CreateJwtToken(user);
            }
    }
}
