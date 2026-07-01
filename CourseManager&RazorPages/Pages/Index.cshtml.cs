using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using CourseManager_RazorPages.Pages.Courses;

namespace CourseManager_RazorPages.Pages
{
    public class IndexModel : PageModel
    {
        public List<Courses.Courses> listele { get; set; } = new List<Courses.Courses>();

        public void OnGet()
        {
            YenileListeyi();
        }

        // Kurs listesini çeker
        private void YenileListeyi()
        {
            string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=CourseManagerDB;Integrated Security=true;TrustServerCertificate=true;";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM Courses";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Courses.Courses kurs = new Courses.Courses
                                {
                                    CoursesID = reader.GetInt32(0).ToString(),
                                    CourseName = reader.IsDBNull(1) ? "" : reader.GetString(1),
                                    Instructor = reader.IsDBNull(2) ? "" : reader.GetString(2),
                                    Duration = reader.IsDBNull(3) ? "0" : reader.GetInt32(3).ToString(),
                                    Price = reader.IsDBNull(4) ? "0" : reader.GetDecimal(4).ToString("N0"), // Ondalıkları göstermeden binlik ayırıcı ile
                                    StartDate = reader.IsDBNull(5) ? "" : reader.GetDateTime(5).ToString("dd.MM.yyyy")
                                };
                                listele.Add(kurs);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Hata durumunda boş liste döner
            }
        }
    }
}
