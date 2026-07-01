using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Data.SqlClient;
using OfficeOpenXml;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace CourseManager_RazorPages.Pages.Reports
{
    public class IndexModel : PageModel
    {
        public int ToplamOgrenci { get; set; } = 0;
        public int ToplamKurs { get; set; } = 0;
        public int ToplamSertifika { get; set; } = 0;
        public decimal ToplamKursDegeri { get; set; } = 0;
        public double OrtalamaKursSuresi { get; set; } = 0;
        public string EnUzunKurs { get; set; } = "Yok";
        public int SonBirAyOgrenci { get; set; } = 0;

        public void OnGet()
        {
            VerileriYukle();
        }

        // Excel indirme işlemi
        public IActionResult OnGetExportExcel()
        {
            VerileriYukle();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var sheet = package.Workbook.Worksheets.Add("Raporlar");

                sheet.Cells["A1:B1"].Merge = true;
                sheet.Cells["A1"].Value = "CourseManager Sistem Analiz Raporu";
                sheet.Cells["A1"].Style.Font.Size = 16;
                sheet.Cells["A1"].Style.Font.Bold = true;

                sheet.Cells["A3"].Value = "Metrik";
                sheet.Cells["B3"].Value = "Değer";
                using (var range = sheet.Cells["A3:B3"])
                {
                    range.Style.Font.Bold = true;
                }

                sheet.Cells["A4"].Value = "Toplam Öğrenci";
                sheet.Cells["B4"].Value = ToplamOgrenci;

                sheet.Cells["A5"].Value = "Toplam Kurs";
                sheet.Cells["B5"].Value = ToplamKurs;

                sheet.Cells["A6"].Value = "Toplam Sertifika";
                sheet.Cells["B6"].Value = ToplamSertifika;

                sheet.Cells["A7"].Value = "Müfredat Değeri (TL)";
                sheet.Cells["B7"].Value = ToplamKursDegeri;
                sheet.Cells["B7"].Style.Numberformat.Format = "#,##0.00";

                sheet.Cells["A8"].Value = "Ortalama Kurs Süresi (Saat)";
                sheet.Cells["B8"].Value = OrtalamaKursSuresi;
                sheet.Cells["B8"].Style.Numberformat.Format = "0.0";

                sheet.Cells["A9"].Value = "En Kapsamlı Kurs";
                sheet.Cells["B9"].Value = EnUzunKurs;

                sheet.Cells["A10"].Value = "Aylık Yeni Kayıt Trendi";
                sheet.Cells["B10"].Value = SonBirAyOgrenci;

                sheet.Column(1).AutoFit();
                sheet.Column(2).AutoFit();

                var bytes = package.GetAsByteArray();
                return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"CourseManager_Rapor_{DateTime.Now:yyyyMMdd}.xlsx");
            }
        }

        // PDF indirme işlemi
        public IActionResult OnGetExportPdf()
        {
            VerileriYukle();
            QuestPDF.Settings.License = LicenseType.Community;

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Arial"));

                    page.Header()
                        .PaddingBottom(1, Unit.Centimetre)
                        .Row(row =>
                        {
                            row.RelativeItem().Column(column =>
                            {
                                column.Item().Text("CourseManager Raporu").SemiBold().FontSize(22).FontColor(Colors.Indigo.Medium);
                                column.Item().Text($"Oluşturulma Tarihi: {DateTime.Now:dd.MM.yyyy HH:mm}").FontSize(9).FontColor(Colors.Grey.Medium);
                            });
                        });

                    page.Content()
                        .Column(column =>
                        {
                            column.Spacing(15);
                            column.Item().Text("Sistem İstatistikleri").SemiBold().FontSize(16).FontColor(Colors.Indigo.Darken2);

                            column.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(3);
                                    columns.RelativeColumn(2);
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Metrik").SemiBold();
                                    header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Değer").SemiBold();
                                });

                                table.Cell().Padding(5).Text("Toplam Öğrenci");
                                table.Cell().Padding(5).Text(ToplamOgrenci.ToString());

                                table.Cell().Padding(5).Text("Toplam Kurs");
                                table.Cell().Padding(5).Text(ToplamKurs.ToString());

                                table.Cell().Padding(5).Text("Toplam Sertifika");
                                table.Cell().Padding(5).Text(ToplamSertifika.ToString());

                                table.Cell().Padding(5).Text("Müfredat Değeri");
                                table.Cell().Padding(5).Text($"{ToplamKursDegeri:N2} TL");

                                table.Cell().Padding(5).Text("Ortalama Kurs Süresi");
                                table.Cell().Padding(5).Text($"{OrtalamaKursSuresi:F1} Saat");

                                table.Cell().Padding(5).Text("En Kapsamlı Kurs");
                                table.Cell().Padding(5).Text(EnUzunKurs);

                                table.Cell().Padding(5).Text("Aylık Kayıt Trendi");
                                table.Cell().Padding(5).Text($"+{SonBirAyOgrenci} Öğrenci");
                            });
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Sayfa ");
                            x.CurrentPageNumber();
                        });
                });
            });

            var bytes = document.GeneratePdf();
            return File(bytes, "application/pdf", $"CourseManager_Rapor_{DateTime.Now:yyyyMMdd}.pdf");
        }

        // Veritabanı verilerini yükler
        private void VerileriYukle()
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
                TempData["RaporHata"] = "Veri okuma hatası: " + ex.Message;
            }
        }
    }
}