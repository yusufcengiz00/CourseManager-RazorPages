using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Data.SqlClient;

namespace CourseManager_RazorPages.Pages.Certificates
{
    public class CreateModel : PageModel
    {
        public Certificates certificateInfo = new Certificates();
        public string errorMessage = "";

        public void OnGet()
        {
            certificateInfo.IssueDate = DateTime.Now.ToString("yyyy-MM-dd");
        }

        public void OnPost()
        {
            certificateInfo.CertificateNo = Request.Form["CertificateNo"];
            certificateInfo.IssueDate = Request.Form["IssueDate"];

            if (string.IsNullOrEmpty(certificateInfo.CertificateNo))
            {
                errorMessage = "Sertifika Numarası zorunludur.";
                return;
            }

            try
            {
                string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=CourseManagerDB;" +
                                          "Integrated Security=true;TrustServerCertificate=true;";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO Certificates (IssueDate, CertificateNo) VALUES (@IssueDate, @CertificateNo)";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@CertificateNo", certificateInfo.CertificateNo);
                        command.Parameters.AddWithValue("@IssueDate", string.IsNullOrEmpty(certificateInfo.IssueDate) ? DBNull.Value : certificateInfo.IssueDate);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = "Veritabanı hatası: " + ex.Message;
                return;
            }

            Response.Redirect("/Certificates/Index");
        }
    }
}