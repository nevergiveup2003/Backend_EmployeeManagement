using EmployeeManagement.Entity;
using EmployeeManagement.Services;
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
            //if (!dbContext.Employees.Any())
            //{
            //    dbContext.Employees.Add(new Employee {  Name = "Employee 1",Phone="2334455" });
            //    dbContext.Employees.Add(new Employee {  Name = "Employee 2", Phone = "45674" });
            //    dbContext.Employees.Add(new Employee {  Name = "Employee 3", Phone = "36765" });
            //}
            if (!dbContext.Users.Any()) {
                var passwordHelper = new PasswordHelper();

                dbContext.Users.Add(new User()
                {
                    Email = "admin@test.com",
                    Password = passwordHelper.HashPassword("12345"),
                    Role = "Admin"
                });
                dbContext.Users.Add(new User()
                {
                    Email = "epl@test.com",
                    Password = passwordHelper.HashPassword("12345"),
                    Role = "Employee"
                });
            }
            
            dbContext.SaveChanges();

        }
    }
}
