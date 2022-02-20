using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using teachersAndStudents.API.Services;
using TeachersAndStudents.models;

namespace teachersAndStudents.API.Modules
{
    public interface IAccountModule
    {
        public Task<string> addStudent(IdentityUser user, Student student, string Password);
        public Task<string> addTeacher(IdentityUser user, Teacher teacher, string Password);
        public Task<string> Login(string UserName, string Password);
        public Task<IEnumerable<Student>> GetStudents();
    }
    public class AccountModule:IAccountModule
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IAuthService authService;
        private readonly AppDbContext appDbContext;

        public AccountModule(UserManager<IdentityUser> userManager, IAuthService authService, AppDbContext appDbContext) {
            this.userManager = userManager;
            this.authService = authService;
            this.appDbContext = appDbContext;
        }
        public async Task<string> addStudent(IdentityUser user, Student student, string Password) {
            await authService.addRoles();
            var result = await userManager.CreateAsync(user, Password);
            if (!result.Succeeded)
            {
                throw new System.Exception(result.Errors.ToString());
            }
            await appDbContext.Students.AddAsync(student);
            await userManager.AddToRoleAsync(user, "Student");
            await appDbContext.SaveChangesAsync();
            return await authService.CreateJwtToken(user);
        }
        public async Task<string> addTeacher(IdentityUser user, Teacher teacher, string Password)
        {
            await authService.addRoles();
            var result = await userManager.CreateAsync(user, Password);
            if (!result.Succeeded)
            {
                throw (System.Exception)result.Errors;
            }
            await appDbContext.Teachers.AddAsync(teacher);
            await userManager.AddToRoleAsync(user, "Student");
            await userManager.AddToRoleAsync(user, "Teacher");
            await appDbContext.SaveChangesAsync();
            return await authService.CreateJwtToken(user);
        }

        public async Task<IEnumerable<Student>> GetStudents()
        {
            return await appDbContext.Students.ToListAsync();
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
