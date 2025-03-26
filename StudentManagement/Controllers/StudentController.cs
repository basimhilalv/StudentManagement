using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Model;
using StudentManagement.Services;

namespace StudentManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentServices _studentServices;
        public StudentController(IStudentServices studentServices) {
            _studentServices = studentServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetStudents()
        {
            var students = await _studentServices.GetStudents();
            return Ok(students);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudent(int id)
        {
            var student = await _studentServices.GetStudent(id);
            if (student == null)
            {
                return NotFound();
            }
            return Ok(student);
        }
        [HttpGet("age/{age}")]
        public async Task<IActionResult> GetStudentByAge(int age)
        {
            var students = await _studentServices.GetStudentByAge(age);
            return Ok(students);
        }

        [HttpPost]
        public async Task<IActionResult> AddStudent(Student student)
        {
            await _studentServices.AddStudent(student);
            return Ok();
        }
        [HttpPut]
        public async Task<IActionResult> UpdateStudent(Student student)
        {
            await _studentServices.UpdateStudent(student);
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            await _studentServices.DeleteStudent(id);
            return NoContent();
        }

    }
}
