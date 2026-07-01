using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Data.SqlClient;

namespace CourseManager_RazorPages.Pages.Students
{
    public class CreateModel : PageModel
    {
        public Students studentInfo = new Students();

        public string errorMessage = "";
        public string successMessage = "";

        public void OnGet()
        {
            studentInfo.RegistrationDate = DateTime.Now.ToString("yyyy-MM-dd");
        }

        public void OnPost()
        {
            studentInfo.FirstName = Request.Form["FirstName"];
            studentInfo.LastName = Request.Form["LastName"];
            studentInfo.Email = Request.Form["Email"];
            studentInfo.Phone = Request.Form["Phone"];
            studentInfo.RegistrationDate = Request.Form["RegistrationDate"];

            // Validasyon
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

                    string sql = "INSERT INTO Students (FirstName, LastName, Email, Phone, RegistrationDate) " +
                                 "VALUES (@FirstName, @LastName, @Email, @Phone, @RegistrationDate)";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@FirstName", studentInfo.FirstName);
                        command.Parameters.AddWithValue("@LastName", studentInfo.LastName);
                        command.Parameters.AddWithValue("@Email", studentInfo.Email);

                        command.Parameters.AddWithValue("@Phone", string.IsNullOrEmpty(studentInfo.Phone) ? DBNull.Value : studentInfo.Phone);
                        command.Parameters.AddWithValue("@RegistrationDate", string.IsNullOrEmpty(studentInfo.RegistrationDate) ? DBNull.Value : studentInfo.RegistrationDate);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = "Veritabanı hatası: " + ex.Message;
                return;
            }

            successMessage = "Yeni öğrenci kaydı başarıyla tamamlandı.";
            Response.Redirect("/Students/Index");
        }
    }
}