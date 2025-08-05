using EmployeeManagement.Data;
using EmployeeManagement.Entity;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> AddDepartment([FromBody] Department model) 
        {
            await departmentRepository.AddAsync(model);
            await departmentRepository.SaveChangesAsync();
            return Ok();
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> UpdateDepartment([FromRoute] int id,[FromBody] Department model)
        {
            var department = await departmentRepository.FindByIdAsync(id);
            department.Name = model.Name;
            departmentRepository.Update(department);
            await departmentRepository.SaveChangesAsync();
            return Ok();
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllDepartment([FromQuery] SearchOptions options)
        {
            var list =await departmentRepository.GetAll();
            var pageData = new PageData<Department>();
            pageData.TotalData = list.Count;
            if (options.PageIndex.HasValue)
            {

                pageData.Data = list.Skip(options!.PageIndex!.Value * options!.PageSize!.Value)
                    .Take(options.PageSize.Value).ToList();
            }
            else
            {
                pageData.Data = list;
            }

                return Ok(pageData);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> DeleteDepartment([FromRoute] int id)
        {
            await departmentRepository.DeleteAsync(id);
           await departmentRepository.SaveChangesAsync();
            return Ok();
        }
    }
}
