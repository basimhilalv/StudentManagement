using StudentManagement.Model;

namespace StudentManagement.Services
{
    public interface IStudentServices
    {
        Task<IEnumerable<Student>> GetStudents();
        Task<Student> GetStudent(int studentID);
        Task<IEnumerable<Student>> GetStudentByAge(int age);
        Task AddStudent(Student student);
        Task UpdateStudent(Student student);
        Task DeleteStudent(int studentID);
    }
}
