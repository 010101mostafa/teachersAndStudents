using System.ComponentModel.DataAnnotations;

namespace TeachersAndStudents.models
{
    public class Class
    {
        public string TeacherId { get; set; }
        [Key]
        public string ClassId { get; set; }
    }
}
