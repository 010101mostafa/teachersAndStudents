﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace teachersAndStudents.API.Entitys
{
    public enum ERole
    {
        Student,
        Teacher 
    }
    public class Signin: Login
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string FullName { get; set; }
        public ERole Role { get; set; }
        public List<string> getRoles() {
            if (Role == ERole.Student)
                return new List<string> { "Student" };
            else if (Role == ERole.Teacher)
                return new List<string> { "Student" , "Teacher" };
            return new List<string>();
        }
    }
}
