using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Data.SqlClient;

namespace CourseManager_RazorPages.Pages.Admin
{
    public class IndexModel : PageModel
    {
        public int ToplamOgrenci { get; set; } = 0;
        public int ToplamKurs { get; set; } = 0;
        public int ToplamSertifika { get; set; } = 0;

        public void OnGet()
        {
            string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=CourseManagerDB;Integrated Security=true;TrustServerCertificate=true;";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Students", connection))
                    {
                        ToplamOgrenci = (int)cmd.ExecuteScalar();
                    }
                    using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Courses", connection))
                    {
                        ToplamKurs = (int)cmd.ExecuteScalar();
                    }
                    using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Certificates", connection))
                    {
                        ToplamSertifika = (int)cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception)
            {
                // Hataları yutuyoruz veya varsayılan 0 değerlerini gösteriyoruz
            }
        }
    }
}
