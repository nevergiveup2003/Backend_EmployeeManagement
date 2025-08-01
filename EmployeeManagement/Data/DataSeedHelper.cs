using EmployeeManagement.Entity;
namespace EmployeeManagement.Data
{
    public class DataSeedHelper
    {
        private readonly AppDbContext dbContext;
        public DataSeedHelper(AppDbContext dbContext) {
            this.dbContext = dbContext;
        }
        public void InsertData()
        {
            if (!dbContext.Employees.Any())
            {
                dbContext.Employees.Add(new Employee {  Name = "Employee 1" });
                dbContext.Employees.Add(new Employee {  Name = "Employee 2" });
                dbContext.Employees.Add(new Employee {  Name = "Employee 3" });
            }
            dbContext.SaveChanges();

        }
    }
}
