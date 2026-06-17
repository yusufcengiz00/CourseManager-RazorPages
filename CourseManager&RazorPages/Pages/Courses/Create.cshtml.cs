using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Data.SqlClient;

namespace CourseManager_RazorPages.Pages.Courses
{
    public class CreateModel : PageModel
    {
        public Courses courseInfo = new Courses();
        public string errorMessage = "";

        public void OnGet()
        {
            courseInfo.StartDate = DateTime.Now.ToString("yyyy-MM-dd");
        }

        public void OnPost()
        {
            courseInfo.CourseName = Request.Form["CourseName"];
            courseInfo.Instructor = Request.Form["Instructor"];
            courseInfo.Duration = Request.Form["Duration"];
            courseInfo.Price = Request.Form["Price"];
            courseInfo.StartDate = Request.Form["StartDate"];

            if (string.IsNullOrEmpty(courseInfo.CourseName) || string.IsNullOrEmpty(courseInfo.Instructor))
            {
                errorMessage = "Kurs Adı and Eğitmen alanları zorunludur.";
                return;
            }

            try
            {
                string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=CourseManagerDB;" +
                                          "Integrated Security=true;TrustServerCertificate=true;";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO Courses (CourseName, Instructor, Duration, Price, StartDate) " +
                                 "VALUES (@CourseName, @Instructor, @Duration, @Price, @StartDate)";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@CourseName", courseInfo.CourseName);
                        command.Parameters.AddWithValue("@Instructor", courseInfo.Instructor);

                        // Sayısal değer boş geldiyse DBNull basıyoruz
                        command.Parameters.AddWithValue("@Duration", string.IsNullOrEmpty(courseInfo.Duration) ? DBNull.Value : Convert.ToInt32(courseInfo.Duration));
                        command.Parameters.AddWithValue("@Price", string.IsNullOrEmpty(courseInfo.Price) ? DBNull.Value : Convert.ToDecimal(courseInfo.Price));
                        command.Parameters.AddWithValue("@StartDate", string.IsNullOrEmpty(courseInfo.StartDate) ? DBNull.Value : courseInfo.StartDate);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = "Veritabanı hatası: " + ex.Message;
                return;
            }

            Response.Redirect("/Courses/Index");
        }
    }
}