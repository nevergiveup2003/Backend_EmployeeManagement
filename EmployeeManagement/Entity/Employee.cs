using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagement.Entity
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string JobTitle { get; set; }
        public int Gender { get; set; }
        [ForeignKey(nameof(Department))]
        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }
        public DateTime JoiningDate { get; set; }
        public DateTime LastWorkingDate { get; set; }
        public DateTime DateOfBirth { get; set; }

        public int? userId { get; set; }
        public User? User { get; set; }
        public int? Salary { get; set; }
    }
}
