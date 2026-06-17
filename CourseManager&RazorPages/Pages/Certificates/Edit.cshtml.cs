using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Data.SqlClient;

namespace CourseManager_RazorPages.Pages.Certificates
{
    public class EditModel : PageModel
    {
        public Certificates certificateInfo = new Certificates();
        public string errorMessage = "";

        public void OnGet()
        {
            string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=CourseManagerDB;" +
                                      "Integrated Security=true;TrustServerCertificate=true;";

            string id = Request.Query["CertificateID"];

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM Certificates WHERE CertificateID=@CertificateID";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@CertificateID", id);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                certificateInfo.CertificateID = reader.GetInt32(0).ToString();
                                certificateInfo.IssueDate = reader.IsDBNull(1) ? "" : reader.GetDateTime(1).ToString("yyyy-MM-dd");
                                certificateInfo.CertificateNo = reader.IsDBNull(2) ? "" : reader.GetString(2);
                            }
                            else
                            {
                                errorMessage = "Sertifika bulunamadı.";
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
            certificateInfo.CertificateID = Request.Form["CertificateID"];
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
                    string sql = "UPDATE Certificates SET IssueDate=@IssueDate, CertificateNo=@CertificateNo WHERE CertificateID=@CertificateID";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@CertificateNo", certificateInfo.CertificateNo);
                        command.Parameters.AddWithValue("@IssueDate", string.IsNullOrEmpty(certificateInfo.IssueDate) ? DBNull.Value : certificateInfo.IssueDate);
                        command.Parameters.AddWithValue("@CertificateID", certificateInfo.CertificateID);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = "Güncelleme sırasında hata oluştu: " + ex.Message;
                return;
            }

            Response.Redirect("/Certificates/Index");
        }
    }
}