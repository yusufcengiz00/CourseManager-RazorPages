using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Data.SqlClient;

namespace CourseManager_RazorPages.Pages.Students
{
    public class EditModel : PageModel
    {
        public Students studentInfo = new Students();
        public string errorMessage = "";
        public string successMessage = "";

        public void OnGet()
        {
            string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=CourseManagerDB;" +
                                      "Integrated Security=true;TrustServerCertificate=true;";

            string id = Request.Query["StudentID"];

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM Students WHERE StudentID=@StudentID";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@StudentID", id);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                studentInfo.StudentID = reader.GetInt32(0).ToString();
                                studentInfo.FirstName = reader.IsDBNull(1) ? "" : reader.GetString(1);
                                studentInfo.LastName = reader.IsDBNull(2) ? "" : reader.GetString(2);
                                studentInfo.Email = reader.IsDBNull(3) ? "" : reader.GetString(3);
                                studentInfo.Phone = reader.IsDBNull(4) ? "" : reader.GetString(4);
                                studentInfo.RegistrationDate = reader.IsDBNull(5) ? "" : reader.GetDateTime(5).ToString("yyyy-MM-dd");
                            }
                            else
                            {
                                errorMessage = "Öğrenci kaydı bulunamadı.";
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
            studentInfo.StudentID = Request.Form["StudentID"];
            studentInfo.FirstName = Request.Form["FirstName"];
            studentInfo.LastName = Request.Form["LastName"];
            studentInfo.Email = Request.Form["Email"];
            studentInfo.Phone = Request.Form["Phone"];
            studentInfo.RegistrationDate = Request.Form["RegistrationDate"];

            if (string.IsNullOrEmpty(studentInfo.FirstName) ||
                string.IsNullOrEmpty(studentInfo.LastName) ||
                string.IsNullOrEmpty(studentInfo.Email))
            {
                errorMessage = "Ad, Soyad ve E-posta alanları zorunludur.";
                return;
            }

            try
            {
                string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=CourseManagerDB;" +
                                          "Integrated Security=true;TrustServerCertificate=true;";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "UPDATE Students SET FirstName=@FirstName, LastName=@LastName, Email=@Email, " +
                                 "Phone=@Phone, RegistrationDate=@RegistrationDate WHERE StudentID=@StudentID";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@FirstName", studentInfo.FirstName);
                        command.Parameters.AddWithValue("@LastName", studentInfo.LastName);
                        command.Parameters.AddWithValue("@Email", studentInfo.Email);

                        command.Parameters.AddWithValue("@Phone", string.IsNullOrEmpty(studentInfo.Phone) ? DBNull.Value : studentInfo.Phone);
                        command.Parameters.AddWithValue("@RegistrationDate", string.IsNullOrEmpty(studentInfo.RegistrationDate) ? DBNull.Value : studentInfo.RegistrationDate);

                        command.Parameters.AddWithValue("@StudentID", studentInfo.StudentID);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = "Güncelleme sırasında hata oluştu: " + ex.Message;
                return;
            }

            successMessage = "Öğrenci bilgileri başarıyla güncellendi.";
            Response.Redirect("/Students/Index");
        }
    }
}