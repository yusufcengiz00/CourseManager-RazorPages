using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Data.SqlClient;

namespace CourseManager_RazorPages.Pages.Courses
{
    public class EditModel : PageModel
    {
        public Courses courseInfo = new Courses();
        public string errorMessage = "";

        public void OnGet()
        {
            string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=CourseManagerDB;" +
                                      "Integrated Security=true;TrustServerCertificate=true;";

            string id = Request.Query["CoursesID"];

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM Courses WHERE CoursesID=@CoursesID";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@CoursesID", id);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                courseInfo.CoursesID = reader.GetInt32(0).ToString();
                                courseInfo.CourseName = reader.IsDBNull(1) ? "" : reader.GetString(1);
                                courseInfo.Instructor = reader.IsDBNull(2) ? "" : reader.GetString(2);
                                courseInfo.Duration = reader.IsDBNull(3) ? "0" : reader.GetInt32(3).ToString();
                                courseInfo.Price = reader.IsDBNull(4) ? "0" : reader.GetDecimal(4).ToString("F2").Replace(",", ".");
                                courseInfo.StartDate = reader.IsDBNull(5) ? "" : reader.GetDateTime(5).ToString("yyyy-MM-dd");
                            }
                            else
                            {
                                errorMessage = "Kurs bulunamadı.";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = "Veri getirilirken hata oluştu: " + ex.Message;
            }
        }

        public void OnPost()
        {
            courseInfo.CoursesID = Request.Form["CoursesID"];
            courseInfo.CourseName = Request.Form["CourseName"];
            courseInfo.Instructor = Request.Form["Instructor"];
            courseInfo.Duration = Request.Form["Duration"];
            courseInfo.Price = Request.Form["Price"];
            courseInfo.StartDate = Request.Form["StartDate"];

            if (string.IsNullOrEmpty(courseInfo.CourseName) || string.IsNullOrEmpty(courseInfo.Instructor))
            {
                errorMessage = "Kurs Adı ve Eğitmen alanları zorunludur.";
                return;
            }

            try
            {
                string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=CourseManagerDB;" +
                                          "Integrated Security=true;TrustServerCertificate=true;";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "UPDATE Courses SET CourseName=@CourseName, Instructor=@Instructor, " +
                                 "Duration=@Duration, Price=@Price, StartDate=@StartDate WHERE CoursesID=@CoursesID";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@CourseName", courseInfo.CourseName);
                        command.Parameters.AddWithValue("@Instructor", courseInfo.Instructor);
                        command.Parameters.AddWithValue("@Duration", string.IsNullOrEmpty(courseInfo.Duration) ? DBNull.Value : Convert.ToInt32(courseInfo.Duration));
                        command.Parameters.AddWithValue("@Price", string.IsNullOrEmpty(courseInfo.Price) ? DBNull.Value : Convert.ToDecimal(courseInfo.Price.Replace(".", ",")));
                        command.Parameters.AddWithValue("@StartDate", string.IsNullOrEmpty(courseInfo.StartDate) ? DBNull.Value : courseInfo.StartDate);
                        command.Parameters.AddWithValue("@CoursesID", courseInfo.CoursesID);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = "Güncelleme sırasında hata oluştu: " + ex.Message;
                return;
            }

            Response.Redirect("/Courses/Index");
        }
    }
}