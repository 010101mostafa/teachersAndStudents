using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
namespace TeachersAndStudents.models
{
    public class User
    {
        [Key, ForeignKey("user")]
        public string UserId { get; set; }
        [MaxLength(60)]
        public string FullName { get; set; }
        // Relations
        public IdentityUser user { get; set; }
    }
}
