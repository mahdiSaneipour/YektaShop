using BN_Project.Core.IService.Admin;
using BN_Project.Core.Response;
using BN_Project.Core.Response.DataResponse;
using BN_Project.Core.Response.Status;
using BN_Project.Core.Tools;
using BN_Project.Domain.Entities;
using BN_Project.Domain.IRepository;
using BN_Project.Domain.ViewModel.Admin;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace BN_Project.Core.Service.Admin
{
    public class AdminServices : IAdminServices
    {
        private readonly IUserRepository _userRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IGalleryRepository _galleryRepository;

        public AdminServices(IUserRepository userRepository, IAccountRepository accountRepository
            , IProductRepository productRepository, ICategoryRepository categoryRepository,
            IGalleryRepository galleryRepository)
        {
            _userRepository = userRepository;
            _accountRepository = accountRepository;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _galleryRepository = galleryRepository;
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

        public async Task<bool> RemoveUserById(int Id)
        {
            var user = await _userRepository.GetUserById(Id);
            if (user == null)
                return false;

            user.IsDelete = true;
            _accountRepository.UpdateUser(user);

            await _userRepository.SaveChanges();

            return true;
        }

        #endregion

        #region Products

        public async Task<DataResponse<IReadOnlyList<ProductListViewModel>>> GetProducts(int pageId = 1)
        {
            DataResponse<IReadOnlyList<ProductListViewModel>> result = new DataResponse<IReadOnlyList<ProductListViewModel>>();

            List<ProductListViewModel> data = new List<ProductListViewModel>();

            var products = await _productRepository.GetProducts();

            if (products == null)
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
                    Category = await _categoryRepository.GetNameById(product.CategoryId),
                    CategoryId = product.CategoryId,
                    Price = product.Price,
                    Name = product.Name,
                    Id = product.Id
                });
            }

            result.Status = Response.Status.Status.Success;
            result.Message = "دریافت محصولات با موفقیت انجام شد";
            result.Data = data.AsReadOnly();

            return result;
        }

        public async Task<BaseResponse> AddProduct(AddProductViewModel addProduct)
        {
            BaseResponse result = new BaseResponse();

            Product product = new Product()
            {
                Description = addProduct.Description,
                CategoryId = addProduct.CategoryId,
                Features = addProduct.Feature,
                Price = addProduct.Price,
                Image = addProduct.Image,
                Name = addProduct.Title
            };

            _productRepository.InsertProduct(product);
            _productRepository.SaveChanges();

            result.Status = Response.Status.Status.Success;
            result.Message = "کاربر با موفقیت افزوده شد";

            return result;
        }

        public async Task<DataResponse<EditProductViewModel>> GetProductForEdit(int productId)
        {
            DataResponse<EditProductViewModel> result = new DataResponse<EditProductViewModel>();

            var product = await _productRepository.GetProductByProductId(productId);

            if (product == null)
                if (product.Id == null)
                {
                    result.Status = Response.Status.Status.Error;
                    result.Message = "خطایی در سیستم رخ داده است";
                }
                else
                {
                    result.Status = Response.Status.Status.NotFound;
                    result.Message = "محصول با این ایدی پیدا نشد";

                    return result;
                }

            EditProductViewModel productMV = new EditProductViewModel()
            {
                Description = product.Description,
                CategoryId = product.CategoryId,
                Feature = product.Features,
                Price = product.Price,
                Image = product.Image,
                Title = product.Name,
                Id = product.Id
            };

            result.Status = Response.Status.Status.Success;
            result.Message = "محصول پیدا شد";
            result.Data = productMV;

            return result;
        }

        #endregion

        #region Categories

        public async Task<Tuple<SelectList, int?>> GetParentCategories(int? selected = 0)
        {
            List<Category> categories = await _categoryRepository.GetAll(c => c.ParentId == null);
            SelectList categoriesSL = null;

            if (selected == 0)
            {
                categoriesSL = new SelectList(categories, "Id", "Title");
            }
            else
            {
                foreach (Category category in categories)
                {
                    if (category.SubCategories != null)
                    {
                        foreach (Category subCategory in category.SubCategories)
                        {
                            if (subCategory.Id == selected)
                            {
                                selected = subCategory.ParentId;
                                break;
                            }
                        }
                    }
                }

                categoriesSL = new SelectList(categories, "Id", "Title", selected);
            }

            return Tuple.Create(categoriesSL, selected);
        }

        public async Task<SelectList> GetSubCategories(int parentId, int? selected = 0)
        {
            List<Category> subCategories = _categoryRepository.GetAll(c => c.ParentId == parentId).Result.ToList();

            SelectList result = null;

            if (selected == 0)
            {
                result = new SelectList(subCategories, "Id", "Title");
            }
            else
            {
                result = new SelectList(subCategories, "Id", "Title", selected);
            }

            return result;
        }

        public async Task<BaseResponse> EditProduct(EditProductViewModel editProduct)
        {
            BaseResponse result = new BaseResponse();
            var product = await _productRepository.GetProductByProductId(editProduct.Id);

            if (product == null)
            {
                result.Status = Status.NotFound;
                result.Message = "محصولی پیدا نشد";
                return result;
            }

            if (product.Image != editProduct.Image)
            {
                Tools.Image.UploadImage.DeleteFile(Directory.GetCurrentDirectory() + "/wwwroot/images/products/thumb/" + product.Image);
                Tools.Image.UploadImage.DeleteFile(Directory.GetCurrentDirectory() + "/wwwroot/images/products/normal/" + product.Image);
            }

            product.Description = editProduct.Description;
            product.CategoryId = editProduct.CategoryId;
            product.Features = editProduct.Feature;
            product.Price = editProduct.Price;
            product.Image = editProduct.Image;
            product.Name = editProduct.Title;
            product.Id = editProduct.Id;

            _productRepository.UpdateProduct(product);
            await _productRepository.SaveChanges();

            result.Status = Status.Success;
            result.Message = "تغییر محصوال با موفقیت انجام شد";

            return result;
        }

        public async Task<BaseResponse> DeleteProductByProductId(int productId)
        {
            BaseResponse result = new BaseResponse();

            var product = await _productRepository.GetProductByProductId(productId);

            if (product == null)
            {
                result.Status = Status.NotFound;
                result.Message = "محصولی با این ایدی پیدا نشد";

                return result;
            }


            product.IsDelete = true;

            _productRepository.UpdateProduct(product);
            await _productRepository.SaveChanges();

            await Console.Out.WriteLineAsync("delete : " + product.IsDelete);

            result.Status = Status.Success;
            result.Message = "محصول با موفقیت حذف شد";

            return result;
        }

        public async Task<List<CategoriesViewModel>> GetAllCategories()
        {
            var items = await _categoryRepository.GetAll();
            List<CategoriesViewModel> categories = new List<CategoriesViewModel>();
            if (items.Count() == 0)
                return categories;
            foreach (var item in items)
            {
                CategoriesViewModel category = new CategoriesViewModel();
                category.Name = item.Title;
                category.Id = item.Id;
                if (item.ParentCategory != null)
                    category.ParentCategoryName = item.ParentCategory.Title;
                categories.Add(category);
            }
            return categories;
        }

        public async Task<bool> AddCategory(AddCategoriesViewModel category)
        {
            Category Category = new Category()
            {
                Title = category.Name,
                ParentId = category.CategoryId
            };
            _categoryRepository.Insert(Category);

            await _categoryRepository.SaveChanges();

            return true;
        }

        public async Task<bool> RemoveCatagory(int Id)
        {
            var item = await _categoryRepository.GetById(Id);
            if (item == null)
                return false;
            item.IsDelete = true;
            _categoryRepository.Update(item);
            await _categoryRepository.SaveChanges();

            return true;
        }

        public async Task<EditCategoryViewModel> GetCategoryById(int Id)
        {
            EditCategoryViewModel EditCategory = new EditCategoryViewModel();
            EditCategory.Categories = await _categoryRepository.GetAll();
            var item = await _categoryRepository.GetById(Id);
            EditCategory.Name = item.Title;
            EditCategory.Id = Id;
            if (item.ParentId != null)
                EditCategory.CategoryId = item.ParentId;

            return EditCategory;
        }

        public async Task<bool> EditCategory(EditCategoryViewModel category)
        {
            var item = await _categoryRepository.GetById(category.Id);
            item.Title = category.Name;
            item.ParentId = category.CategoryId;

            _categoryRepository.Update(item);
            await _categoryRepository.SaveChanges();

            return true;

        }
        #endregion

        #region Gallery
        public async Task<GalleryImagesViewModel> GetGalleryByProductId(int Id)
        {
            var items = await _galleryRepository.GetAll(n => n.ProductId == Id);
            List<GalleryViewModel> Gallery = new List<GalleryViewModel>();
            foreach (var item in items)
            {
                GalleryViewModel gallery = new GalleryViewModel()
                {
                    Id = item.Id,
                    ImageName = item.ImageName
                };
                Gallery.Add(gallery);
            }
            GalleryImagesViewModel galleryImages = new GalleryImagesViewModel();
            galleryImages.Galleries = Gallery;
            galleryImages.PriductId = Id;

            return galleryImages;
        }

        public async Task<bool> AddGalleryImage(AddGalleryViewModel gallery)
        {
            if (gallery.ImageName == null || gallery.ProductId == 0)
                return false;
            ProductGallery productGallery = new ProductGallery()
            {
                ImageName = gallery.ImageName,
                ProductId = gallery.ProductId
            };
            _galleryRepository.Insert(productGallery);

            await _galleryRepository.SaveChanges();

            return true;
        }

        public async Task<int> RemoveGalleryImage(int imageId)
        {
            var item = await _galleryRepository.GetById(imageId);
            item.IsDelete = true;

            _galleryRepository.Update(item);
            await _galleryRepository.SaveChanges();

            return item.ProductId;
        }
        #endregion
    }
}
