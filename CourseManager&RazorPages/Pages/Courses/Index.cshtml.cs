using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace CourseManager_RazorPages.Pages.Courses
{
    public class IndexModel : PageModel
    {
        public List<Courses> listele { get; set; } = new List<Courses>();
        public string AramaKelimesi { get; set; } = "";

        public void OnGet(string search)
        {
            if (!string.IsNullOrEmpty(search))
            {
                AramaKelimesi = search;
            }

            YenileListeyi(AramaKelimesi);
        }

        private void YenileListeyi(string filtre)
        {
            string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=CourseManagerDB;" +
                                      "Integrated Security=true;TrustServerCertificate=true;";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM Courses";

                    if (!string.IsNullOrEmpty(filtre))
                    {
                        sql += " WHERE CourseName LIKE @filtre OR Instructor LIKE @filtre";
                    }

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        if (!string.IsNullOrEmpty(filtre))
                        {
                            command.Parameters.AddWithValue("@filtre", "%" + filtre + "%");
                        }

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Courses kurs = new Courses
                                {
                                    CoursesID = reader.GetInt32(0).ToString(),
                                    CourseName = reader.IsDBNull(1) ? "" : reader.GetString(1),
                                    Instructor = reader.IsDBNull(2) ? "" : reader.GetString(2),
                                    Duration = reader.IsDBNull(3) ? "0" : reader.GetInt32(3).ToString(),
                                    Price = reader.IsDBNull(4) ? "0" : reader.GetDecimal(4).ToString("F2"),
                                    StartDate = reader.IsDBNull(5) ? "" : reader.GetDateTime(5).ToString("dd.MM.yyyy")
                                };
                                listele.Add(kurs);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Kurslar listelenirken hata oluştu: " + ex.Message;
            }
        }
    }
}