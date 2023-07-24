using BN_Project.Core.DTOs.UserProfile;
using BN_Project.Core.Response.DataResponse;

namespace BN_Project.Core.IService.Profile
{
    public interface IProfileService
    {
        public DataResponse<UserInformation> GetAllUserInformation(string Id);
    }
}
