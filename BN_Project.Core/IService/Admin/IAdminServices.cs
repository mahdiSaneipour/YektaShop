using BN_Project.Core.Response;
using BN_Project.Core.Response.DataResponse;
using BN_Project.Domain.Entities;
using BN_Project.Domain.ViewModel.Admin;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Drawing;

namespace BN_Project.Core.IService.Admin
{
    public interface IAdminServices
    {
        #region Users

        public Task<DataResponse<IReadOnlyList<UserListViewModel>>> GetUsersForAdmin(int pageId);

        public Task<BaseResponse> AddUserFromAdmin(AddUserViewModel addUser);
        public Task<EditUserViewModel> GetUserById(int Id);
        public Task<bool> RemoveUserById(int Id);
        public Task<BaseResponse> EditUsers(EditUserViewModel user);

        #endregion

        #region Product

        public Task<DataResponse<IReadOnlyList<ProductListViewModel>>> GetProducts(int pageId = 1);

        public Task<BaseResponse> AddProduct(AddProductViewModel addProduct);

        public Task<DataResponse<EditProductViewModel>> GetProductForEdit(int productId);

        public Task<BaseResponse> EditProduct(EditProductViewModel editProduct);

        public Task<BaseResponse> DeleteProductByProductId(int productId);

        public Task<DataResponse<List<string>>> SearchProductByName(string name);

        #endregion

        #region Categories

        public Task<Tuple<SelectList, int?>> GetParentCategories(int? selected = 0);

        public Task<SelectList> GetSubCategories(int parentId, int? selected = 0);

        public Task<List<CategoriesViewModel>> GetAllCategories();

        public Task<bool> AddCategory(AddCategoriesViewModel category);

        public Task<bool> RemoveCatagory(int Id);

        public Task<EditCategoryViewModel> GetCategoryById(int Id);

        public Task<bool> EditCategory(EditCategoryViewModel category);

        #endregion

        #region Colors

        public Task<DataResponse<IReadOnlyList<ListColorViewModel>>> GetAllColors(int pageId);

        public Task<BaseResponse> AddColor(AddColorViewModel addColor);

        #endregion

        #region Gallery

        public Task<GalleryImagesViewModel> GetGalleryByProductId(int Id);

        public Task<bool> AddGalleryImage(AddGalleryViewModel gallery);

        public Task<int> RemoveGalleryImage(int imageId);

        #endregion
    }
}
