using System.ComponentModel.DataAnnotations;

namespace teachersAndStudents.API.Entitys
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
