using EmployeeManagement.Data;
using EmployeeManagement.Entity;
using System.Security.Claims;

namespace EmployeeManagement.Services
{
    public class UserHelper
    {
        private readonly IRepository<User> userRepo;
        private readonly IRepository<Employee> emRepo;

        public UserHelper(IRepository<User>userRepo,IRepository<Employee>emRepo)
        {
            this.userRepo = userRepo;
            this.emRepo = emRepo;
        }

        public async Task<int> GetUserId(ClaimsPrincipal userClaim)
        {
            var email = userClaim!.FindFirstValue(ClaimTypes.Name);
            var user = (await userRepo.GetAll(x => x.Email == email)).First();
            var employee = (await emRepo.GetAll(x => x.userId == user.Id)).FirstOrDefault();
            return user.Id;
        }
        public async Task<int?> GetEmployeeId(ClaimsPrincipal userClaim)
        {
            var email = userClaim!.FindFirstValue(ClaimTypes.Name);
            var user = (await userRepo.GetAll(x => x.Email == email)).First();
            var employee = (await emRepo.GetAll(x => x.userId == user.Id)).FirstOrDefault();
            return employee?.Id;
        }
        public async Task<bool> IsAdmin(ClaimsPrincipal userClaim)
        {
            var role = userClaim!.FindFirstValue(ClaimTypes.Role);
            return role == "Admin";
        }
    }
}
