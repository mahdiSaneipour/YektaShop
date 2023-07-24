using BN_Project.Core.DTOs.User;
using BN_Project.Core.Response;
using BN_Project.Core.Response.DataResponse;
using BN_Project.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BN_Project.Core.IService.Account
{
    public interface IAccountServices
    {
        public Task<DataResponse<UserEntity>> CreateUser(RegisterUser register);

        public Task<DataResponse<UserEntity>> LoginUser(LoginUser login);

        public Task<BaseResponse> IsTokenTrue(string token);
    }
}
