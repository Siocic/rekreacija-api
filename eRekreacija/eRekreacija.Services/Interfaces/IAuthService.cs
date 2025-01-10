using eRekreacija.Models.Models;
using eRekreacija.Services.Database;
using Microsoft.AspNetCore.Identity;

namespace eRekreacija.Services.Interfaces
{
    public interface IAuthService
    {
        Task<IdentityResult> RegisterUser(RegisterRequest request,int flag);
        Task<string> LoginAsync(string email, string password);
        Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();
        Task<IEnumerable<IdentityRole>> GetAllRolesAsync();

    }
}
