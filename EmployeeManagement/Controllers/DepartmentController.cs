using EmployeeManagement.Data;
using EmployeeManagement.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IRepository<Department> departmentRepository;

        public DepartmentController(IRepository<Department> departmentRepository)
        {
            this.departmentRepository = departmentRepository;
        }
        [HttpPost]
        public async Task<IActionResult> AddDepartment([FromBody] Department model) 
        {
            await departmentRepository.AddAsync(model);
            await departmentRepository.SaveChangesAsync();
            return Ok();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartment([FromRoute] int id,[FromBody] Department model)
        {
            var department = await departmentRepository.FindByIdAsync(id);
            department.Name = model.Name;
            departmentRepository.Update(department);
            await departmentRepository.SaveChangesAsync();
            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> GetAllDepartment()
        {
            var list =await departmentRepository.GetAll();
            return Ok(list);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment([FromRoute] int id)
        {
            await departmentRepository.DeleteAsync(id);
           await departmentRepository.SaveChangesAsync();
            return Ok();
        }
    }
}
