using System;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CourseManager_RazorPages.Pages.Certificates
{
    public class DeleteModel : PageModel
    {
        public void OnGet()
        {
            string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=CourseManagerDB;" +
                                      "Integrated Security=true;TrustServerCertificate=true;";

            string id = Request.Query["CertificateID"];

            if (!string.IsNullOrEmpty(id))
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string sql = "DELETE FROM Certificates WHERE CertificateID = @CertificateID";

                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@CertificateID", id);
                            command.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Sertifika silinirken hata oluştu: " + ex.Message;
                }
            }

            Response.Redirect("/Certificates/Index");
        }
    }
}