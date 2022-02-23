
namespace TeachersAndStudents.models
{
    public class Student:User
    {
        // Relations
        public Class Class { get; set; }
        public string ClassId { get; set; }
    }
}
