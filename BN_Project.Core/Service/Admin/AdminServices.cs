using BN_Project.Core.IService.Admin;
using BN_Project.Core.Response;
using BN_Project.Core.Tools;
using BN_Project.Domain.Entities;
using BN_Project.Domain.IRepository;
using BN_Project.Domain.ViewModel.Admin;

namespace BN_Project.Core.Service.Admin
{
    public class AdminServices : IAdminServices
    {
        private readonly IUserRepository _userRepository;
        private readonly IAccountRepository _accountRepository;

        public AdminServices(IUserRepository userRepository, IAccountRepository accountRepository)
        {
            _userRepository = userRepository;
            _accountRepository = accountRepository;
        }

        #region Users

        public async Task<BaseResponse> AddUserFromAdmin(AddUserViewModel addUser)
        {
            var result = new BaseResponse();

            if (_userRepository.IsEmailExist(addUser.Email).Result)
            {
                result.Status = Response.Status.Status.AlreadyHave;
                result.Message = "کاربری با این ایمیل موجد است";

                return result;
            }
            else if (_userRepository.IsPhoneNumberExist(addUser.PhoneNumber).Result)
            {
                result.Status = Response.Status.Status.AlreadyHavePhoneNumber;
                result.Message = "کاربری با این شماره موبایل موجد است";

                return result;
            }

            UserEntity user = new UserEntity()
            {
                Password = addUser.Password.EncodePasswordMd5(),
                ActivationCode = Tools.Tools.GenerateUniqCode(),
                PhoneNumber = addUser.PhoneNumber,
                Avatar = addUser.Avatar,
                Email = addUser.Email,
                Name = addUser.Name,
                IsActive = true
            };

            await _userRepository.AddUserFromAdmin(user);
            await _accountRepository.SaveChanges();

            result.Status = Response.Status.Status.Success;
            result.Message = "کاربر با موفقیت اضافه شد";

            return result;
        }

        public async Task<BaseResponse> EditUser(EditUserViewModel user)
        {
            var result = new BaseResponse();

            //if (_userRepository.IsEmailExist(user.Email).Result)
            //{
            //    result.Status = Response.Status.Status.AlreadyHave;
            //    result.Message = "کاربری با این ایمیل موجود است";

            //    return result;
            //}
            //else if (_userRepository.IsPhoneNumberExist(user.PhoneNumber).Result)
            //{
            //    result.Status = Response.Status.Status.AlreadyHavePhoneNumber;
            //    result.Message = "کاربری با این شماره موبایل موجود است";

            //    return result;
            //}

            var item = await _accountRepository.GetUserById(user.Id);

            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/avatar/normal", item.Avatar).RemoveFile();
            Path.Combine(Directory.GetCurrentDirectory(), "wwroot/images/avatar/thumb", item.Avatar).RemoveFile();


            item.Id = user.Id;

            if (user.Password != null)
                item.Password = user.Password.EncodePasswordMd5();

            item.PhoneNumber = user.PhoneNumber;
            item.Avatar = user.Avatar;
            item.Email = user.Email;
            item.Name = user.Name;


            _accountRepository.UpdateUser(item);
            await _accountRepository.SaveChanges();

            result.Status = Response.Status.Status.Success;
            result.Message = "کاربر با موفقیت اضافه شد";

            return result;
        }

        public async Task<EditUserViewModel> GetUserById(int Id)
        {
            var item = await _userRepository.GetUserById(Id);
            EditUserViewModel EditUserVM = new EditUserViewModel()
            {
                Id = Id,
                Avatar = item.Avatar,
                Name = item.Name,
                Email = item.Email,
                PhoneNumber = item.PhoneNumber
            };

            return EditUserVM;
        }

        public async Task<IReadOnlyList<UserListViewModel>> GetUsersForAdmin(int pageId)
        {
            List<UserListViewModel> result = new List<UserListViewModel>();

            var users = await _userRepository.GetAllUsers();

            if (users == null)
            {
                return result;
            }

            int take = 10;
            int skip = (pageId - 1) * take;

            var lUsers = users.ToList().Skip(skip).Take(take).OrderByDescending(u => u.Id).ToList();

            foreach (var user in lUsers)
            {
                result.Add(new UserListViewModel()
                {
                    PhoneNumber = (user.PhoneNumber != null) ? user.PhoneNumber : "---",
                    IsActive = user.IsActive,
                    Email = user.Email,
                    Name = (user.Name != null) ? user.Name : "---",
                    Id = user.Id
                });
            }

            return result.AsReadOnly();
        }

        public async Task<bool> RemoveUserById(int Id)
        {
            var user = await _userRepository.GetUserById(Id);
            if (user == null)
                return false;
            _userRepository.RemoveUser(user);

            await _userRepository.SaveChanges();

            return true;
        }

        #endregion
    }
}
