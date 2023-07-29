using BN_Project.Core.IService.Admin;
using BN_Project.Core.Response;
using BN_Project.Core.Response.DataResponse;
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
        private readonly IProductRepository _productRepository;

        public AdminServices(IUserRepository userRepository, IAccountRepository accountRepository
            , IProductRepository productRepository)
        {
            _userRepository = userRepository;
            _accountRepository = accountRepository;
            _productRepository = productRepository;
        }

        #region Users

        public async Task<BaseResponse> AddUserFromAdmin(AddUserViewModel addUser)
        {
            var result = new BaseResponse();

            if(_userRepository.IsEmailExist(addUser.Email).Result)
            {
                result.Status = Response.Status.Status.AlreadyHave;
                result.Message = "کاربری با این ایمیل موجد است";

                return result;
            } else if (_userRepository.IsPhoneNumberExist(addUser.PhoneNumber).Result)
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

        public async Task<DataResponse<IReadOnlyList<ProductListViewModel>>> GetProducts(int pageId = 1)
        {
            DataResponse<IReadOnlyList<ProductListViewModel>> result = new DataResponse<IReadOnlyList<ProductListViewModel>>();

            List<ProductListViewModel> data = new List< ProductListViewModel>();

            var products = await _productRepository.GetProducts();

            if(products == null)
            {
                result.Status = Response.Status.Status.NotFound;
                result.Message = "محصولی وجود ندارد";

                return result;
            }

            int take = 10;
            int skip = (pageId - 1) * take;

            var lProducts = products.ToList().Skip(skip).Take(take).OrderByDescending(u => u.Id).ToList();

            foreach (var product in lProducts)
            {
                data.Add(new ProductListViewModel()
                {
                    /*Category = */
                    CategoryId = product.CategoryId,
                    Name = product.Name,
                    Price = product.Price,
                });
            }

            result.Status = Response.Status.Status.Success;
            result.Message = "دریافت محصولات با موفقیت انجام شد";
            result.Data = data.AsReadOnly();

            return result;
        }

        public async Task<DataResponse<IReadOnlyList<UserListViewModel>>> GetUsersForAdmin(int pageId)
        {
            DataResponse<IReadOnlyList<UserListViewModel>> result = new DataResponse<IReadOnlyList<UserListViewModel>>();

            List<UserListViewModel> data = new List<UserListViewModel>();

            var users = await _userRepository.GetAllUsers();

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

        #endregion
    }
}
