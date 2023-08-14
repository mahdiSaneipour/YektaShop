using BN_Project.Core.Response;
using BN_Project.Core.Response.DataResponse;
using BN_Project.Domain.Entities;
using BN_Project.Domain.ViewModel.Admin;
using BN_Project.Domain.ViewModel.Product;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BN_Project.Core.Services.Interfaces
{
    public interface IProductServices
    {
        #region Product

        public Task<DataResponse<IReadOnlyList<ProductListViewModel>>> GetProducts(int pageId = 1);

        public Task<DataResponse<List<ListProductViewModel>>> GetProductsListShowByCategoryId(int categoryId);

        public Task<BaseResponse> AddProduct(AddProductViewModel addProduct);

        public Task<DataResponse<EditProductViewModel>> GetProductForEdit(int productId);

        public Task<BaseResponse> EditProduct(EditProductViewModel editProduct);

        public Task<BaseResponse> DeleteProductByProductId(int productId);

        public Task<DataResponse<List<string>>> SearchProductByName(string name);

        public Task<BaseResponse> ProductReadyForAddAndEdit();

        public Task<DataResponse<ShowProductViewModel>> GetProductForShowByProductId(int productId);

        #endregion

        #region Categories

        public Task<Tuple<SelectList, int?>> GetParentCategories(int? selected = 0);

        public Task<SelectList> GetSubCategories(int parentId, int? selected = 0);

        public Task<List<CategoriesViewModel>> GetAllCategories();

        public Task<bool> AddCategory(AddCategoriesViewModel category);

        public Task<bool> RemoveCatagory(int Id);

        public Task<EditCategoryViewModel> GetCategoryById(int Id);

        public Task<bool> EditCategory(EditCategoryViewModel category);

        public Task<List<Category>> GetAllCategoriesForHeader();

        public Task<DataResponse<List<string>>> SearchCategoriesByName(string name);

        public Task<int> GetCategoryIdByCategoryName(string name);

        #endregion

        #region Colors

        public Task<DataResponse<IReadOnlyList<ListColorViewModel>>> GetAllColors(int pageId);

        public Task<BaseResponse> AddColor(AddColorViewModel addColor);

        public Task<BaseResponse> EditColor(EditColorViewModel editColor);

        public Task<DataResponse<EditColorViewModel>> GetEditColor(int colorId);

        public Task<BaseResponse> ColorReadyForAddAndEdit();

        public Task<BaseResponse> DeleteColorById(int colorId);

        public Task<long> GetPriceByColorId(int colorId);

        public Task<long> GetCountByColorId(int colorId);

        #endregion

        #region Gallery

        public Task<GalleryImagesViewModel> GetGalleryByProductId(int Id);

        public Task<bool> AddGalleryImage(AddGalleryViewModel gallery);

        public Task<int> RemoveGalleryImage(int imageId);

        #endregion

        #region Discount

        public Task<List<DiscountViewModel>> GetAllDiscounts();

        public Task<List<ProductsForDiscountViewModel>> GetAllProductsForDiscount();
        public Task<bool> AddDiscount(AddDiscountViewModel discount);

        public Task<bool> RemoveDiscount(int Id);

        public Task<List<ProductsForDiscountViewModel>> GetListOfProducts(int Id);

        public Task<EditDiscountViewModel> GetDiscountForEdit(int Id);

        public Task<bool> EditDiscount(EditDiscountViewModel editDiscount);
        #endregion
    }
}