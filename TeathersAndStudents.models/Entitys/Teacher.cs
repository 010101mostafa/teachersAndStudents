using System.Collections.Generic;
namespace TeachersAndStudents.models
{
    public class Teacher : User
    {
        // Relations
        public IEnumerable<Class> Class { get; set; }
    }
}
