using BN_Project.Core.IService.Account;
using BN_Project.Core.Response;
using BN_Project.Core.Response.DataResponse;
using BN_Project.Core.Response.Status;
using BN_Project.Core.Tools;
using BN_Project.Domain.Entities;
using BN_Project.Domain.IRepository;
using BN_Project.Domain.ViewModel.Account;
using BN_Project.Domain.ViewModel.UserProfile;

namespace BN_Project.Core.Service.Account
{
    public class AccountServices : IAccountServices
    {
        private readonly IAccountRepository _accountRepository;

        public AccountServices(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
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

        async void IAccountServices.UpdateUser(UpdateUserInfoViewModel user)
        {
            UserEntity userE = new UserEntity();

            userE = await _accountRepository.GetSingle(n => n.Id == user.Id);
            userE.Name = user.FullName;
            userE.PhoneNumber = user.PhoneNumber;
            userE.Password = user.Password;
            _accountRepository.Update(userE);

            await _accountRepository.SaveChanges();
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
    }
}