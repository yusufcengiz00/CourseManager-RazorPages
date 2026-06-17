using System;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CourseManager_RazorPages.Pages.Courses
{
    public class DeleteModel : PageModel
    {
        public void OnGet()
        {
            string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=CourseManagerDB;" +
                                      "Integrated Security=true;TrustServerCertificate=true;";

            string id = Request.Query["CoursesID"];

            if (!string.IsNullOrEmpty(id))
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string sql = "DELETE FROM Courses WHERE CoursesID = @CoursesID";

                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@CoursesID", id);
                            command.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Kurs silinirken hata oluştu: " + ex.Message;
                }
            }

            Response.Redirect("/Courses/Index");
        }
    }
}