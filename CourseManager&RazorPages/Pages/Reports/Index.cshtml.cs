using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Data.SqlClient;

namespace CourseManager_RazorPages.Pages.Reports
{
    public class IndexModel : PageModel
    {
        // Temel Adetler
        public int ToplamOgrenci { get; set; } = 0;
        public int ToplamKurs { get; set; } = 0;
        public int ToplamSertifika { get; set; } = 0;

        // 4 Analiz Metriği
        public decimal ToplamKursDegeri { get; set; } = 0;
        public double OrtalamaKursSuresi { get; set; } = 0;
        public string EnUzunKurs { get; set; } = "Yok";
        public int SonBirAyOgrenci { get; set; } = 0;

        public void OnGet()
        {
            string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=CourseManagerDB;" +
                                      "Integrated Security=true;TrustServerCertificate=true;";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // --- TEMEL SAYILAR ---
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

                    // --- 4 ANALİZ METRİĞİ ---
                    using (SqlCommand cmd = new SqlCommand("SELECT SUM(Price) FROM Courses", connection))
                    {
                        var res = cmd.ExecuteScalar();
                        ToplamKursDegeri = res != DBNull.Value ? Convert.ToDecimal(res) : 0;
                    }
                    using (SqlCommand cmd = new SqlCommand("SELECT AVG(CAST(Duration AS FLOAT)) FROM Courses", connection))
                    {
                        var res = cmd.ExecuteScalar();
                        OrtalamaKursSuresi = res != DBNull.Value ? Convert.ToDouble(res) : 0;
                    }
                    using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 CourseName FROM Courses ORDER BY CAST(Duration AS INT) DESC", connection))
                    {
                        var res = cmd.ExecuteScalar();
                        EnUzunKurs = res != null ? res.ToString() : "Tanımlanmadı";
                    }
                    using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Students WHERE RegistrationDate >= DATEADD(month, -1, GETDATE())", connection))
                    {
                        SonBirAyOgrenci = (int)cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["RaporHata"] = "Veriler işlenirken hata oluştu: " + ex.Message;
            }
        }
    }
}