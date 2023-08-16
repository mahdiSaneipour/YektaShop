using BN_Project.Core.Response;
using BN_Project.Core.Response.DataResponse;
using BN_Project.Core.Response.Status;
using BN_Project.Core.Services.Interfaces;
using BN_Project.Core.Tools;
using BN_Project.Domain.Entities;
using BN_Project.Domain.Enum.Ticket;
using BN_Project.Domain.IRepository;
using BN_Project.Domain.ViewModel.Account;
using BN_Project.Domain.ViewModel.Admin;
using BN_Project.Domain.ViewModel.UserProfile;

namespace BN_Project.Core.Services.Implementations
{
    public class UserServices : IUserServices
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITicketRepository _ticketRepository;

        public UserServices(
            IAccountRepository accountRepository,
            IUserRepository userRepository,
            ITicketRepository ticketRepository)
        {
            _accountRepository = accountRepository;
            _userRepository = userRepository;
            _ticketRepository = ticketRepository;
        }

        public async Task<DataResponse<UserEntity>> CreateUser(RegisterUserViewModel register)
        {
            DataResponse<UserEntity> result = new DataResponse<UserEntity>();

            var user = await _accountRepository.GetSingle(n => n.Email == register.Email);

            if (user != null)
            {
                result.Status = Response.Status.Status.AlreadyHave;
                result.Message = "این ایمیل قبلا استفاده شده";

                return result;
            }

            UserEntity userEntity = new UserEntity()
            {
                Email = register.Email,
                Create = DateTime.Now,
                ActivationCode = Tools.Tools.GenerateUniqCode(),
                Password = Tools.Tools.EncodePasswordMd5(register.Password)
            };

            await _accountRepository.Insert(userEntity);
            await _accountRepository.SaveChanges();

            result.Status = Response.Status.Status.Success;
            result.Message = "ورود با موفقیت انجام شد";
            result.Data = userEntity;

            return result;
        }

        public async Task<DataResponse<UserInformationViewModel>> GetUserInformationById(int Id)
        {
            DataResponse<UserInformationViewModel> result = new DataResponse<UserInformationViewModel>();
            var user = await _accountRepository.GetSingle(n => n.Id == Id);
            result.Data = new UserInformationViewModel();

            if (user != null)
            {
                result.Data.Password = user.Password;
                result.Data.FullName = user.Name;
                result.Data.PhoneNumber = user.PhoneNumber;
                result.Data.Email = user.Email;
                result.Message = "با موفقیت انجام شد!";
                result.Status = Status.Success;
            }
            else
            {
                result.Message = "با شکست مواجه شد!";
                result.Status = Status.NotFound;
            }
            return result;
        }

        public async Task<DataResponse<UserEntity>> ForgotPassword(string email)
        {
            DataResponse<UserEntity> result = new DataResponse<UserEntity>();
            var user = await _accountRepository.GetSingle(n => n.Email == email);

            if (user == null)
            {
                result.Status = Response.Status.Status.NotValid;
                result.Message = "ایمیل وارد شده معتبر نمیباشد";

                return result;
            }

            result.Status = Response.Status.Status.Success;
            result.Message = "کاربری با موفقیت پیدا شد";
            result.Data = user;

            return result;
        }

        public async Task<BaseResponse> IsTokenTrue(string token)
        {
            BaseResponse result = new BaseResponse();
            await Console.Out.WriteLineAsync("token : " + token);
            var user = await _accountRepository.GetSingle(n => n.ActivationCode == token);

            if (user == null)
            {
                result.Status = Response.Status.Status.NotFound;
                result.Message = "توکن صحیح نمیباشد";

                return result;
            }

            user.IsActive = true;

            ChangeActivationCode(user);

            await _accountRepository.SaveChanges();

            return result;
        }

        public async Task<DataResponse<UserEntity>> LoginUser(LoginUserViewModel login)
        {
            DataResponse<UserEntity> result = new DataResponse<UserEntity>();

            var user = await _accountRepository.GetSingle(n => n.Email == login.Email);

            if (user == null)
            {
                result.Status = Response.Status.Status.NotFound;
                result.Message = "ایمیل و رمز عبور با هم سازگار نیستند";

                return result;
            }

            if (user.IsActive == false)
            {
                result.Status = Status.NotActive;
                result.Message = "لطفا ابتدا ایمیل خود را تایید کنید";

                return result;
            }

            if (CheckPassword(user.Id, login.Password).Result == false)
            {
                result.Status = Response.Status.Status.NotMatch;
                result.Message = "ایمیل و رمز عبور با هم سازگار نیستند";

                return result;
            }

            result.Message = "ورود با موفقیت انجام شد";
            result.Status = Response.Status.Status.Success;
            result.Data = user;

            return result;
        }

        public async Task<bool> CheckPassword(int id, string password)
        {
            var user = await _accountRepository.GetSingle(n => n.Id == id);

            if (user == null)
            {
                return false;
            }

            if (password.EncodePasswordMd5() != user.Password)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> CheckToken(string token)
        {
            var user = await _accountRepository.GetSingle(n => n.ActivationCode == token);

            if (user != null)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> ResetPassword(ResetPasswordViewModel resetPassword)
        {
            var user = await _accountRepository.GetSingle(n => n.ActivationCode == resetPassword.Token);

            if (user == null)
            {
                return false;
            }

            user.Password = resetPassword.Password.EncodePasswordMd5();

            _accountRepository.Update(user);
            ChangeActivationCode(user);

            return true;
        }

        public async void ChangeActivationCode(UserEntity user)
        {
            user.ActivationCode = Tools.Tools.GenerateUniqCode();

            _accountRepository.Update(user);
            await _accountRepository.SaveChanges();
        }

        public async void DeleteAccount(int Id)
        {
            var item = await _accountRepository.GetSingle(n => n.Id == Id);
            _accountRepository.Delete(item);

            await _accountRepository.SaveChanges();
        }

        public async Task<bool> ChangeUserPassword(UserLoginInformationViewModel user)
        {
            if (user.Password != null && user.NewPass != null && user.ConfirmNewPass != null)
            {
                var currentUser = await _accountRepository.GetSingle(n => n.Id == user.UserId);

                string EncodedPassword = user.Password.EncodePasswordMd5();
                if (EncodedPassword == currentUser.Password)
                {
                    currentUser.Password = user.NewPass.EncodePasswordMd5();

                    _accountRepository.Update(currentUser);
                    await _accountRepository.SaveChanges();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public async Task<BaseResponse> AddUserFromAdmin(AddUserViewModel addUser)
        {
            var result = new BaseResponse();

            if (_userRepository.IsEmailExist(addUser.Email).Result)
            {
                result.Status = Response.Status.Status.AlreadyHave;
                result.Message = "کاربری با این ایمیل موجد است";

                return result;
            }
            else if (await _userRepository.IsPhoneNumberExist(addUser.PhoneNumber))
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

            await _userRepository.Insert(user);
            await _accountRepository.SaveChanges();

            result.Status = Response.Status.Status.Success;
            result.Message = "کاربر با موفقیت اضافه شد";

            return result;
        }

        public async Task<BaseResponse> EditUsers(EditUserViewModel user)
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

            var item = await _accountRepository.GetSingle(n => n.Id == user.Id);
            try
            {
                Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/avatar/normal", item.Avatar).RemoveFile();
                Path.Combine(Directory.GetCurrentDirectory(), "wwroot/images/avatar/thumb", item.Avatar).RemoveFile();
            }
            catch
            {
            }

            item.Id = user.Id;

            if (user.Password != null)
                item.Password = user.Password.EncodePasswordMd5();

            item.PhoneNumber = user.PhoneNumber;
            item.Avatar = user.Avatar;
            item.Email = user.Email;
            item.Name = user.Name;


            _accountRepository.Update(item);
            await _accountRepository.SaveChanges();

            result.Status = Response.Status.Status.Success;
            result.Message = "کاربر با موفقیت اضافه شد";

            return result;
        }

        public async Task<EditUserViewModel> GetUserById(int Id)
        {
            var item = await _userRepository.GetSingle(n => n.Id == Id);
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

        public async Task<DataResponse<IReadOnlyList<UserListViewModel>>> GetUsersForAdmin(int pageId)
        {
            DataResponse<IReadOnlyList<UserListViewModel>> result = new DataResponse<IReadOnlyList<UserListViewModel>>();

            List<UserListViewModel> data = new List<UserListViewModel>();

            var users = await _userRepository.GetAll();

            if (users == null)
            {
                result.Status = Response.Status.Status.NotFound;
                result.Message = "کاربری وجود ندارد";

                return result;
            }

            int take = 10;
            int skip = (pageId - 1) * take;

            var lUsers = users.ToList().Skip(skip).Take(take).OrderByDescending(u => u.Id).ToList();

            foreach (var user in lUsers)
            {
                data.Add(new UserListViewModel()
                {
                    PhoneNumber = (user.PhoneNumber != null) ? user.PhoneNumber : "---",
                    IsActive = user.IsActive,
                    Email = user.Email,
                    Name = (user.Name != null) ? user.Name : "---",
                    Id = user.Id
                });
            }

            result.Status = Response.Status.Status.Success;
            result.Message = "دریافت کاربران با موفقیت انجام شد";
            result.Data = data.AsReadOnly();

            return result;
        }

        public async Task<bool> RemoveUserById(int userId)
        {
            var user = await _userRepository.GetSingle(n => n.Id == userId);
            if (user == null)
                return false;

            user.IsDelete = true;
            _accountRepository.Update(user);

            await _userRepository.SaveChanges();

            return true;
        }

        public async Task<bool> UpdateUserFullName(UpdateUserInfoViewModel userInfo)
        {
            if (userInfo.FullName != null && userInfo.Id != 0)
            {
                var user = await _userRepository.GetSingle(n => n.Id == userInfo.Id);
                user.Name = userInfo.FullName;
                _userRepository.Update(user);
                await _userRepository.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> UpdatePhoneNumber(UpdateUserInfoViewModel userInfo)
        {
            if (userInfo.PhoneNumber != null && userInfo.Id != 0)
            {
                var user = await _userRepository.GetSingle(n => n.Id == userInfo.Id);
                user.PhoneNumber = userInfo.PhoneNumber;

                _userRepository.Update(user);
                await _userRepository.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }
        }

        #region Tickets
        public async Task<List<TicketViewModel>> GetAllTickets()
        {
            var items = await _ticketRepository.GetAllWithRelation();
            List<TicketViewModel> ticetks = new List<TicketViewModel>();
            ticetks.AddRange(items.Select(n => new TicketViewModel
            {
                Id = n.Id,
                CreatedDate = n.Create,
                LastUpdatedTime = n.LastUpadate,
                Section = n.Section.Name,
                Status = n.Status.GetDisplayName(),
                Subject = n.Subject
            }).ToList());

            return ticetks;
        }

        public async Task<bool> CloseTicket(int Id)
        {
            if (Id == 0)
                return false;
            var item = await _ticketRepository.GetSingle(n => n.Id == Id);
            item.Status = TicketStatus.Closed;

            await _ticketRepository.SaveChanges();
            return true;
        }

        #endregion
    }
}