using StudentManagement.Model;
using System.Data.SqlClient;

namespace StudentManagement.Services
{
    public class StudentServices : IStudentServices
    {
        private readonly string _connnectionString;
        public StudentServices(IConfiguration configuration)
        {
            _connnectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task AddStudent(Student student)
        {
            using (SqlConnection connection = new SqlConnection(_connnectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand("INSERT INTO Students (FirstName, LastName, Age) VALUES (@FirstName, @LastName, @Age); SELECT SCOPE_IDENTITY();", connection))
                {
                    command.Parameters.AddWithValue("@FirstName", student.FirstName);
                    command.Parameters.AddWithValue("@LastName", student.LastName);
                    command.Parameters.AddWithValue("@Age", student.Age);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task DeleteStudent(int studentID)
        {
            using (SqlConnection connection = new SqlConnection(_connnectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand("DELETE FROM Students WHERE StudentID = @StudentID", connection))
                {
                    command.Parameters.AddWithValue("@StudentID", studentID);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<Student> GetStudent(int studentID)
        {
            using (SqlConnection connection = new SqlConnection(_connnectionString))
            {
                await connection.OpenAsync();
                using(SqlCommand command = new SqlCommand("SELECT * FROM Students WHERE StudentID = @StudentID", connection))
                {
                    command.Parameters.AddWithValue("@StudentID", studentID);
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Student
                            {
                                StudentID = (int)reader["StudentID"],
                                FirstName = reader["FirstName"] as string,
                                LastName = reader["LastName"] as string,
                                Age = reader["Age"] as int?
                            };
                        }
                        return null;
                    }
                }
            }
        }

        public async Task<IEnumerable<Student>> GetStudents()
        {
            List<Student> students = new List<Student>();
            using (SqlConnection connection = new SqlConnection(_connnectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand("SELECT * FROM Students", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        
                        while ( await reader.ReadAsync())
                        {
                            students.Add(new Student
                            {
                                StudentID = (int)reader["StudentID"],
                                FirstName = reader["FirstName"] as string,
                                LastName = reader["LastName"] as string,
                                Age = reader["Age"] as int?
                            });
                        }
                        
                    }
                }
            }
            return students;
        }

        public async Task UpdateStudent(Student student)
        {
            using(SqlConnection connection = new SqlConnection(_connnectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand("UPDATE Students SET FirstName = @FirstName, LastName = @LastName, Age = @Age WHERE StudentID = @StudentID", connection))
                {
                    command.Parameters.AddWithValue("@FirstName", student.FirstName);
                    command.Parameters.AddWithValue("@LastName", student.LastName);
                    command.Parameters.AddWithValue("@Age", student.Age);
                    command.Parameters.AddWithValue("@StudentID", student.StudentID);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

    }
}
