using StudentManagement.Model;
using System.Data;
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

        private async Task<DataSet> GetStudentFromDatabase(string filterexpression = null)
        {
            DataSet dataSet = new DataSet();
            using (SqlConnection connection = new SqlConnection(_connnectionString))
            {
                await connection.OpenAsync();
                string selectCommandText = "SELECT * FROM Students";
                if (!string.IsNullOrEmpty(filterexpression))
                {
                    selectCommandText += " WHERE " + filterexpression;
                }
                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(selectCommandText, connection))
                {
                    dataAdapter.Fill(dataSet, "Students");
                }
            }
            return dataSet;
        }

        public async Task<IEnumerable<Student>> GetStudentByAge(int age)
        {
            DataSet dataSet = await GetStudentFromDatabase($"Age = {age}");
            List<Student> students = new List<Student>();
            if (dataSet.Tables.Contains("Students"))
            {
                foreach (DataRow row in dataSet.Tables["Students"].Rows)
                {
                    students.Add(new Student
                    {
                        StudentID = Convert.ToInt32(row["StudentID"]),
                        FirstName = row["FirstName"].ToString(),
                        LastName = row["LastName"].ToString(),
                        Age = Convert.ToInt32(row["Age"])
                    });
                }
            }
            return students;
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
            DataSet dataSet = await GetStudentFromDatabase($"StudentID = {studentID}");
            if (dataSet.Tables.Contains("Students") && dataSet.Tables["Students"].Rows.Count > 0)
            {
                dataSet.Tables["Students"].Rows[0].Delete();
                await UpdateDatabase(dataSet);
            }
        }

        private async Task UpdateDatabase(DataSet dataSet)
        {
            using (SqlConnection connection = new SqlConnection(_connnectionString))
            {
                await connection.OpenAsync();
                string selectCommandText = "SELECT * FROM Students";
                using (SqlDataAdapter adapter = new SqlDataAdapter(selectCommandText, connection))
                {
                    SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                    adapter.Update(dataSet, "Students");
                }
            }
        }

        public async Task<Student> GetStudent(int studentID)
        {
            DataSet dataSet = await GetStudentFromDatabase($"StudentID = {studentID}");
            if(dataSet.Tables.Contains("Students") && dataSet.Tables["Students"].Rows.Count > 0)
            {
                DataRow row = dataSet.Tables["Students"].Rows[0];
                return new Student
                {
                    StudentID = Convert.ToInt32(row["StudentID"]),
                    FirstName = row["FirstName"].ToString(),
                    LastName = row["LastName"].ToString(),
                    Age = Convert.ToInt32(row["Age"])
                };
            }
            return null;
        }

        public async Task<IEnumerable<Student>> GetStudents()
        {

            DataSet dataSet = await GetStudentFromDatabase();
            List<Student> students = new List<Student>();
            if (dataSet.Tables.Contains("Students"))
            {
                foreach (DataRow row in dataSet.Tables["Students"].Rows)
                {
                    students.Add(new Student
                    {
                        StudentID = Convert.ToInt32(row["StudentID"]),
                        FirstName = row["FirstName"].ToString(),
                        LastName = row["LastName"].ToString(),
                        Age = Convert.ToInt32(row["Age"])
                    });
                }
            }
            return students;
        }

        public async Task UpdateStudent(Student student)
        {
            DataSet dataSet = await GetStudentFromDatabase($"StudentID = {student.StudentID}");
            if (dataSet.Tables.Contains("Students") && dataSet.Tables["Students"].Rows.Count > 0)
            {
                DataRow row = dataSet.Tables["Students"].Rows[0];
                row["FirstName"] = student.FirstName;
                row["LastName"] = student.LastName;
                row["Age"] = student.Age;
                await UpdateDatabase(dataSet);
            }
        }

    }
}
