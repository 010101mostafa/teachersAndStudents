using System.Collections.Generic;
namespace TeachersAndStudents.models
{
    public class Teacher : User
    {
        // Relations
        public IList<Class> Class { get; set; }
    }
}
