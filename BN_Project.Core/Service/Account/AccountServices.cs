using BN_Project.Core.IService.Account;
using BN_Project.Core.Response;
using BN_Project.Core.Response.DataResponse;
using BN_Project.Core.Response.Status;
using BN_Project.Domain.Entities;
using BN_Project.Domain.IRepository;
using BN_Project.Domain.ViewModel.Account;
using BN_Project.Domain.ViewModel.UserProfile;
using Toplearn2.Application.Tools;

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

            var user = await _accountRepository.GetUserByEmail(register.Email);

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
                ActivationCode = Tools.GenerateUniqCode(),
                Password = Tools.EncodePasswordMd5(register.Password)
            };

            user = await _accountRepository.RegisterUsere(userEntity);
            await _accountRepository.SaveChanges();

            result.Status = Response.Status.Status.Success;
            result.Message = "ورود با موفقیت انجام شد";
            result.Data = user;

            return result;
        }

        public async Task<DataResponse<UserInformationViewModel>> GetUserInformationById(int Id)
        {
            DataResponse<UserInformationViewModel> result = new DataResponse<UserInformationViewModel>();
            var user =await _accountRepository.GetUserById(Id);
            result.Data = new UserInformationViewModel();

            if (user != null)
            {
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
            var user = await _accountRepository.GetUserByEmail(email);

            if (user != null)
            {
                result.Status = Response.Status.Status.NotValid;
                result.Message = "ایمیل وارد شده معتبر نمیباشد";
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
            var user = _accountRepository.GetUserByToken(token).Result;

            if (user == null)
            {
                result.Status = Response.Status.Status.NotFound;
                result.Message = "توکن صحیح نمیباشد";

                return result;
            }

            user.IsActive = true;

            await _accountRepository.SaveChanges();

            return result;
        }

        public async Task<DataResponse<UserEntity>> LoginUser(LoginUserViewModel login)
        {
            DataResponse<UserEntity> result = new DataResponse<UserEntity>();

            var user = await _accountRepository.GetUserByEmail(login.Email);

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

            string password = Tools.EncodePasswordMd5(login.Password);

            if (user.Password != password)
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


        public Task<DataResponse<UserInformationViewModel>> GetUserByEmail(string email)
        {
            throw new NotImplementedException();
        }

        async void IAccountServices.UpdateUser(UpdateUserInfoViewModel user)
        {
            UserEntity userE = new UserEntity();

            userE = _accountRepository.GetUserById(user.Id).Result;
            userE.Name = user.FullName;
            userE.PhoneNumber = user.PhoneNumber;
            _accountRepository.UpdateUser(userE);

            await _accountRepository.SaveChanges();
        }
    }
}