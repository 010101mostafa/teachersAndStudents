using System.ComponentModel.DataAnnotations;

namespace TeachersAndStudents.models
{
    public class Teacher
    {
        [Key]
        [Required]
        public string UserId { get; set; }
        [MaxLength(60)]
        public string FullName { get; set; }
    }
}
