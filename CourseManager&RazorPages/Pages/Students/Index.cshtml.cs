using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace CourseManager_RazorPages.Pages.Students
{
    public class IndexModel : PageModel
    {
        public List<Students> listele { get; set; } = new List<Students>();

        public string AramaKelimesi { get; set; } = "";

        public void OnGet(string search)
        {
            if (!string.IsNullOrEmpty(search))
            {
                AramaKelimesi = search;
            }

            YenileListeyi(AramaKelimesi);
        }


        public IActionResult OnGetDelete(string StudentID)
        {
            if (string.IsNullOrEmpty(StudentID))
            {
                return Page();
            }

            string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=CourseManagerDB;" +
                                      "Integrated Security=true;TrustServerCertificate=true;";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "DELETE FROM Students WHERE StudentID = @StudentID";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@StudentID", StudentID);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Silme işlemi sırasında hata oluştu: " + ex.Message;
            }

            return RedirectToPage("/Students/Index");
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

                    string sql = "SELECT * FROM Students";

                    if (!string.IsNullOrEmpty(filtre))
                    {
                        sql += " WHERE FirstName LIKE @filtre OR LastName LIKE @filtre OR Email LIKE @filtre";
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
                                Students ogrenci = new Students
                                {
                                    StudentID = reader.GetInt32(0).ToString(),
                                    FirstName = reader.IsDBNull(1) ? "" : reader.GetString(1),
                                    LastName = reader.IsDBNull(2) ? "" : reader.GetString(2),
                                    Email = reader.IsDBNull(3) ? "" : reader.GetString(3),
                                    Phone = reader.IsDBNull(4) ? "" : reader.GetString(4),
                                    RegistrationDate = reader.IsDBNull(5) ? "" : reader.GetDateTime(5).ToString("dd.MM.yyyy")
                                };
                                listele.Add(ogrenci);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Veri listelenirken bir hata oluştu: " + ex.Message;
            }
        }
    }
}