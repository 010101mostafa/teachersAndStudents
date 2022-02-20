using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeachersAndStudents.models
{
    public class Login
    {
        [Required]
        public string  UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string password { get; set; }
        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}
