namespace CourseManager_RazorPages.Pages.Courses
{
    public class Courses
    {
        public string? CoursesID { get; set; }
        public string? CourseName { get; set; }
        public string? Instructor { get; set; }
        public string? Duration { get; set; }
        public string? Price { get; set; }
        public string StartDate { get; set; } = "";
    }
}