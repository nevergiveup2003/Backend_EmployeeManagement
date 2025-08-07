using EmployeeManagement.Data;
using EmployeeManagement.Entity;
using EmployeeManagement.Models;
using EmployeeManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace EmployeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveController : ControllerBase
    {
        private readonly IRepository<Leave> leaveRepo;
        private readonly UserHelper userHelper;
        private readonly IRepository<Attendance> attendanceRepo;

        public LeaveController(IRepository<Leave> leaveRepo,UserHelper userHelper, IRepository<Attendance> attendanceRepo)
        {
            this.leaveRepo = leaveRepo;
            this.userHelper = userHelper;
            this.attendanceRepo = attendanceRepo;
        }


        [HttpPost("apply")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> ApplyLeave([FromBody] LeaveDto model)
        {
            var employeeId = await userHelper.GetEmployeeId(User);
            var leave = new Leave()
            {
                EmployeeId = employeeId.Value,
                Reason = model.Reason,
                Type = (int)model.Type!,
                LeaveDate = model.LeaveDate.Value,
                Status = (int)LeaveStatus.Pending
            };
            await leaveRepo.AddAsync(leave);
            await leaveRepo.SaveChangesAsync();
            return Ok();
        }
        [HttpPost("updateStatus")]
        [Authorize(Roles = "Employee,Admin")]
        public async Task<IActionResult> UpdateLeaveStatus([FromBody] LeaveDto model)
        {
            var leave = await leaveRepo.FindByIdAsync(model.Id!.Value);
            var isAdmin = await userHelper.IsAdmin(User);
            if (model.Id == null || model.Status == null)
            {
                return BadRequest("Thiếu Id hoặc Status.");
            }

            if (leave == null)
            {
                return NotFound("Không tìm thấy đơn nghỉ phép.");
            }

            if (isAdmin)
            {
                leave.Status = model.Status.Value;
                if(model.Status.Value == (int)LeaveStatus.Accepted)
                {
                    await attendanceRepo.AddAsync(new Attendance()
                    {
                        Date =leave.LeaveDate,
                        EmployeeId = leave.EmployeeId,
                        Type =(int) AttendanceType.Leave
                    });
                }
            }
            else
            {
                if (model.Status == (int)LeaveStatus.Canelled)
                {
                    leave.Status = model.Status.Value;
                }
                else
                {
                    return BadRequest("Bạn không có quyền thay đổi trạng thái này.");
                }
            }

            await leaveRepo.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        [Authorize(Roles = "Employee,Admin")]

        public async Task<IActionResult> List([FromRoute] SearchOptions options)
        {
           List<Leave>  list =await leaveRepo.GetAll();
            if (await userHelper.IsAdmin(User))
            { 
                list = await leaveRepo.GetAll();
            } 
            else
            {
                var employeeId = await userHelper.GetEmployeeId(User);
                list = await leaveRepo.GetAll(x => x.EmployeeId == employeeId.Value);
            }
                var pageData = new PageData<Leave>();
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
    }
}
