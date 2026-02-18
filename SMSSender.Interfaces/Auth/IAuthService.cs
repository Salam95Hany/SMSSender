using SMSSender.Entities.Auth;
using SMSSender.Entities.Common;
using SMSSender.Entities.Contracts.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Interfaces.Auth
{
    public interface IAuthService
    {
        Task<ApiResponseModel<List<UserWithRolesDto>>> GetAllUsers();
        Task<ApiResponseModel<UserWithRolesDto>> GetUserInfoById(string UserId);
        Task<ApiResponseModel<ApplicationUserRespone>> AdminLogin(LoginModel request);
        Task<ApiResponseModel<string>> CreateUser(AddUserModel model);
        Task<ApiResponseModel<string>> EditUser(AddUserModel model);
        Task<ApiResponseModel<string>> DeleteUser(string userId);
        Task<ApiResponseModel<string>> AdminLogout(string UserId);
        Task<ApiResponseModel<AdminUser>> GetUserById(string docId);
        Task<ApiResponseModel<string>> EditUserProfile(AddUserModel model);
        Task<ApiResponseModel<string>> ChangeUserPassword(AddUserModel model);
    }
}
