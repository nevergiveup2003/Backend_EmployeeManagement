namespace EmployeeManagement.Entity
{
    public class Attendance
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Type { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

    }
    public enum AttendanceType
    {
        Present = 1,
        Leave = 2
    }
}
