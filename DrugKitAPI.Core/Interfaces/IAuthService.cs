using DrugKitAPI.Core.DTOs.Auth;

namespace DrugKitAPI.Core.Interfaces
{
    public interface IAuthService
    {
        Task<AuthDTO> RegisterAsync(RegisterDTO model);
        Task<AuthDTO> LoginAsync(LoginDTO model);
    }
}