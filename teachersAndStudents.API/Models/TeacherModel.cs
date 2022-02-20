using System.Threading.Tasks;
using TeachersAndStudents.models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace teachersAndStudents.API.Models
{
    public interface ITeacherModel
    {
        public Task<IEnumerable<Student>> getStudent();
        public Task AddToAClass(Student student);
        public Task addClass(Class _class);
        public Task<IEnumerable<Class>> getClass(string TeacherId);
    }
    public class TeacherModel : ITeacherModel
    {
        private readonly AppDbContext appDbContext;

        public TeacherModel(AppDbContext appDbContext) {
            this.appDbContext = appDbContext;
        }
        public async Task addClass(Class _class)
        {
            await appDbContext.Class.AddAsync(_class);
            await appDbContext.SaveChangesAsync();
        }

        public async Task AddToAClass(Student student)
        {
            (await appDbContext.Students.FirstOrDefaultAsync(x=>x.UserId==student.UserId))
                .ClassId = student.ClassId;
            await appDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Class>> getClass(string TeacherId)
        {
            return await appDbContext.Class.Where(x => x.TeacherId == TeacherId).ToArrayAsync();
        }

        public async Task<IEnumerable<Student>> getStudent()
        {
            return await appDbContext.Students.ToArrayAsync();
        }
    }
}
