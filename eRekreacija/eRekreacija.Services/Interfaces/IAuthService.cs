using eRekreacija.Models.DTOs;
using eRekreacija.Models.Models;
using eRekreacija.Services.Database;
using Microsoft.AspNetCore.Identity;

namespace eRekreacija.Services.Interfaces
{
    public interface IAuthService
    {
        Task<IdentityResult> RegisterUser(RegisterRequest request,int flag);
        Task<string> LoginAsync(string email, string password);
        Task<List<ApplicationUserDTO>> GetAllUserOfRolePravnoLice();
        Task<List<ApplicationUserDTO>> GetAllPravnoLiceThatNotApprovedYet();
        Task<List<ApplicationUserDTO>> GetAllUserOfRoleFizikoLice();
        Task<ApplicationUserDTO> GetUser(string userId);
        Task<bool> EditProfile(ApplicationUserDTO model);
        Task<int> ChangePassword(ChangePasswordDTO model, string userID);
        Task<bool> ApproveRegistration(string userId);
    }
}
