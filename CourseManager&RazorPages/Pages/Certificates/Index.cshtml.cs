using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace CourseManager_RazorPages.Pages.Certificates
{
    public class IndexModel : PageModel
    {
        public List<Certificates> listele { get; set; } = new List<Certificates>();
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
                    string sql = "SELECT * FROM Certificates";

                    if (!string.IsNullOrEmpty(filtre))
                    {
                        sql += " WHERE CertificateNo LIKE @filtre";
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
                                Certificates sertifika = new Certificates
                                {
                                    CertificateID = reader.GetInt32(0).ToString(),
                                    IssueDate = reader.IsDBNull(1) ? "" : reader.GetDateTime(1).ToString("dd.MM.yyyy"),
                                    CertificateNo = reader.IsDBNull(2) ? "" : reader.GetString(2)
                                };
                                listele.Add(sertifika);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Sertifikalar listelenirken hata oluştu: " + ex.Message;
            }
        }
    }
}