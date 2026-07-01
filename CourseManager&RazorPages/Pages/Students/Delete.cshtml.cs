using System;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CourseManager_RazorPages.Pages.Students
{
    public class DeleteModel : PageModel
    {
        public Students studentInfo = new Students();

        public void OnGet()
        {
            string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=CourseManagerDB;Integrated Security=true;TrustServerCertificate=true;";
            string id = Request.Query["StudentID"];

            if (!string.IsNullOrEmpty(id))
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string sql = "DELETE FROM Students WHERE StudentID = @StudentID";

                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@StudentID", id);
                            command.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Silme işlemi başarısız: " + ex.Message;
                }
            }

            Response.Redirect("/Students/Index");
        }
    }
}