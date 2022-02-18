using System.ComponentModel.DataAnnotations;

namespace teachersAndStudents.API.Entitys
{
    public class Class
    {
        public string TeacherId { get; set; }
        [Key]
        public string ClassId { get; set; }
    }
}
