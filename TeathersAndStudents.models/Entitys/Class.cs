using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TeachersAndStudents.models
{
    public class Class
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }

        // Relations
        [ForeignKey("Teacher")]
        public string TeacherId { get; set; }
        public Teacher Teacher { get; set; }
        public IList<Student> students { get; set; }
    }
}
