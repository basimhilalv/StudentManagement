using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Model
{
    public class Student
    {
        public int StudentID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? Age { get; set; }
    }
}
