using EmployeeManagement.Data;
using EmployeeManagement.Entity;
using EmployeeManagement.Models;
using EmployeeManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly IRepository<Attendance> attendanceRepo;
        private readonly UserHelper userHelper;

        public AttendanceController(IRepository<Attendance>attendanceRepo,UserHelper userHelper)
        {
            this.attendanceRepo = attendanceRepo;
            this.userHelper = userHelper;
        }
        [HttpPost("mark-present")]
        [Authorize(Roles ="Employee")]
        public async Task<IActionResult> MarkAttendance()
        {
            var emplyeeId = await userHelper.GetEmployeeId(User);
            var attendanceList = await attendanceRepo.GetAll(x => x.EmployeeId == emplyeeId.Value && DateTime.Compare(x.Date.Date, DateTime.UtcNow.Date) == 0);
            if(attendanceList.Count > 0)
            {
                return BadRequest("Already Present");
            }
            var attendance = new Attendance()
            {
                Date = DateTime.UtcNow,
                EmployeeId = emplyeeId.Value,
                Type = (int)AttendanceType.Present,
            };
            await attendanceRepo.AddAsync(attendance);
            await attendanceRepo.SaveChangesAsync();
            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> GetAttendanceHistory([FromQuery] SearchOptions options)
        {
            if(!await userHelper.IsAdmin(User))
            {
                options.EmployeeId = await userHelper.GetEmployeeId(User);

            }
            var list = await attendanceRepo.GetAll(x => x.EmployeeId == options.EmployeeId.Value);
            var pageData = new PageData<Attendance>();
            pageData.TotalData = list.Count;
            if (options.PageIndex.HasValue)
            {
                list = list.Skip(options.PageIndex.Value * options.PageSize.Value)
                    .Take(options.PageSize.Value).ToList();
                pageData.Data = list;
            }
                return Ok(pageData);
        }
    }
}
