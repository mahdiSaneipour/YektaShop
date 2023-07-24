using BN_Project.Core.DTOs.UserProfile;
using BN_Project.Core.IService.Profile;
using BN_Project.Core.Response.DataResponse;
using BN_Project.Domain.IRepository;

namespace BN_Project.Core.Service.UserProfile
{
    public class ProfileService : IProfileService
    {
        private readonly IUserInformationRepository _userInformation;
        public ProfileService(IUserInformationRepository UserInformation)
        {
            _userInformation = UserInformation;
        }
        public DataResponse<UserInformation> GetAllUserInformation(string Id)
        {
            DataResponse<UserInformation> result = new DataResponse<UserInformation>();
            var userInformaiton = _userInformation.GetUserInformationByToken(Convert.ToInt32(Id)).Result;

            if (userInformaiton != null)
                return result;

            result.Data.Email = userInformaiton.Email;
            result.Data.FullName = userInformaiton.Name;
            result.Data.PhoneNumber = userInformaiton.PhoneNumber;

            return result;
        }
    }
}
