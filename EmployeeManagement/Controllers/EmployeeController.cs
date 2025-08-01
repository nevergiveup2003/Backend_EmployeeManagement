using EmployeeManagement.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly AppDbContext dbContext;
        public EmployeeController(AppDbContext dbContext) {
            this.dbContext = dbContext;
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(dbContext.Employees.ToList());
        }
    }
}
