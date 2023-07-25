using BN_Project.Core.DTOs.User;
using BN_Project.Core.DTOs.UserProfile;
using BN_Project.Core.IService.Account;
using BN_Project.Core.Response;
using BN_Project.Core.Response.DataResponse;
using BN_Project.Core.Response.Status;
using BN_Project.Domain.Entities;
using BN_Project.Domain.IRepository;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public async Task<DataResponse<UserEntity>> CreateUser(RegisterUser register)
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

            /*if (user.Id == 0)
            {
                result.Status = Response.Status.Status.Error;
                result.Message = "خطایی رخ داده, لطفا بعدا امتحان کنید";

                return result;
            }*/

            result.Status = Response.Status.Status.Success;
            result.Message = "ورود با موفقیت انجام شد";
            result.Data = user;

            return result;
        }

        public async Task<DataResponse<UserInformation>> GetUserByEmail(string email)
        {
            DataResponse<UserInformation> result = new DataResponse<UserInformation>();
            var user = await _accountRepository.GetUserByEmail(email);

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

        public async Task<DataResponse<UserEntity>> LoginUser(LoginUser login)
        {
            DataResponse<UserEntity> result = new DataResponse<UserEntity>();

            var user = await _accountRepository.GetUserByEmail(login.Email);

            if (user == null)
            {
                result.Status = Response.Status.Status.NotFound;
                result.Message = "کاربری با این ایمیل پیدا نشد";

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
    }
}