using System.Threading.Tasks;
using TeachersAndStudents.models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace teachersAndStudents.API.Models
{
    public interface ITeacherModel
    {
        public Task<List<Student>> getStudent();
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
            var s = await appDbContext.Students.FirstOrDefaultAsync(s => s.UserId == student.UserId);
            if (s.ClassId == default(string))
                s.ClassId = student.ClassId;
            else
                throw new System.Exception($"the student {s.FullName} is already in a Class");
            await appDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Class>> getClass(string TeacherId)
        {
            return (await appDbContext.Teachers.Include(t=>t.Class).FirstOrDefaultAsync(t => t.UserId == TeacherId)).Class;
        }

        public async Task<List<Student>> getStudent()
        {
            return await appDbContext.Students.Include(s => s.user).Include(s=>s.Class).ToListAsync();
            
        }
    }
}
