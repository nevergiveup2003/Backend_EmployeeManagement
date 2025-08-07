using EmployeeManagement.Data;
using EmployeeManagement.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IRepository<Employee> emRepo;
        private readonly IRepository<Department> deRepo;

        public DashboardController(IRepository<Employee> emRepo,IRepository<Department>deRepo)
        {
            this.emRepo = emRepo;
            this.deRepo = deRepo;
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> TotalSalary()
        {
            var deList = await deRepo.GetAll();
            var empList =await emRepo.GetAll();
            var totalSalary = empList.Sum(x =>x.Salary ?? 0);
            var employeeCount = empList.Count;
            var departmentCount = deList.Count();
            return Ok(new
            {
                TotalSalary = totalSalary,
                employeeCount = employeeCount,
                departmentCount = departmentCount,
            });
        }
    }
}
