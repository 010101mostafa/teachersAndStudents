using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using teachersAndStudents.API.Models;
using TeachersAndStudents.models;
using Microsoft.AspNetCore.Authorization;
using System;

namespace teachersAndStudents.API.Controllers
{
    [Authorize(Roles ="Teacher")]
    [ApiController]
    [Route("[Controller]")]
    public class TeacherController:ControllerBase
    {
        private readonly ITeacherModel teacherModel;

        public TeacherController(ITeacherModel teacherModel) {
            this.teacherModel = teacherModel;
        }
        [HttpPost("addClass")]
        public async Task<IActionResult> addClass(Class _class)
        {
            if (!ModelState.IsValid) { 
                return BadRequest(ModelState);
            }
            try
            {
                await teacherModel.addClass(_class);
                return Ok();
            }
            catch (Exception e) {
                return BadRequest();
            }
        }
        [HttpPost("AddToAClass")]
        public async Task<IActionResult> AddToAClass(Student student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await teacherModel.AddToAClass(student);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
        [HttpGet("getClass")]
        public async Task<IActionResult> getClass()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                string TeacherId = HttpContext.User.FindFirst("uid").Value;
                return Ok(await teacherModel.getClass(TeacherId));
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
        [HttpGet("getStudent")]
        public async Task<IActionResult> getStudent()
        {
            try
            {
                return Ok(await teacherModel.getStudent());
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
    }
}
