namespace SchoolManagementConsuming.Models
{
    public class AcademicCalendar
    {
        public int Id { get; set; }
        public string AcademicYear { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<Event> Events { get; set; } = new List<Event>();
    }
}
