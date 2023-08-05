using BN_Project.Core.Response;
using BN_Project.Core.Response.DataResponse;
using BN_Project.Core.Response.Status;
using BN_Project.Core.Services.Interfaces;
using BN_Project.Domain.Entities;
using BN_Project.Domain.IRepository;
using BN_Project.Domain.ViewModel.Admin;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BN_Project.Core.Services.Implementations
{
    public class ProductServices : IProductServices
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IColorRepository _colorRepository;
        private readonly IGalleryRepository _galleryRepository;

        public ProductServices(IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IGalleryRepository galleryRepository,
            IColorRepository colorRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _galleryRepository = galleryRepository;
            _colorRepository = colorRepository;
        }

        #region Products

        public async Task<DataResponse<IReadOnlyList<ProductListViewModel>>> GetProducts(int pageId = 1)
        {
            DataResponse<IReadOnlyList<ProductListViewModel>> result = new DataResponse<IReadOnlyList<ProductListViewModel>>();

            List<ProductListViewModel> data = new List<ProductListViewModel>();

            var products = await _productRepository.GetAll();

            if (products == null)
            {
                result.Status = Status.NotFound;
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

            result.Status = Status.Success;
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

            await _productRepository.Insert(product);
            await _productRepository.SaveChanges();

            result.Status = Status.Success;
            result.Message = "کاربر با موفقیت افزوده شد";

            return result;
        }

        public async Task<DataResponse<EditProductViewModel>> GetProductForEdit(int productId)
        {
            DataResponse<EditProductViewModel> result = new DataResponse<EditProductViewModel>();

            var product = await _productRepository.GetSingle(n => n.Id == productId);

            if (product == null)
                if (product.Id == 0)
                {
                    result.Status = Status.Error;
                    result.Message = "خطایی در سیستم رخ داده است";
                }
                else
                {
                    result.Status = Status.NotFound;
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

            result.Status = Status.Success;
            result.Message = "محصول پیدا شد";
            result.Data = productMV;

            return result;
        }

        public async Task<DataResponse<List<string>>> SearchProductByName(string name)
        {
            DataResponse<List<string>> result = new DataResponse<List<string>>();
            List<string> data = await _productRepository.SearchProductAndReturnName(name);

            if (data == null)
            {
                result.Status = Status.NotFound;
                result.Message = "محصولی با این نام پیدا نشد";
            }
            else
            {
                result.Status = Status.Success;
                result.Message = "محصول ها با موفقیت پیدا شدند";
                result.Data = data;
            }

            return result;
        }

        #endregion

        #region Categories

        public async Task<Tuple<SelectList, int?>> GetParentCategories(int? selected = 0)
        {
            List<Category> categories = (List<Category>)await _categoryRepository.GetAll(n => n.ParentId == null);
            SelectList categoriesSL = null;

            if (selected == 0)
            {
                categoriesSL = new SelectList(categories, "Id", "Title");
            }
            else
            {
                selected = await _categoryRepository.GetParentIdBySubCategoryId((int)selected);

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
            var product = await _productRepository.GetSingle(n => n.Id == editProduct.Id);

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

            _productRepository.Update(product);
            await _productRepository.SaveChanges();

            result.Status = Status.Success;
            result.Message = "تغییر محصوال با موفقیت انجام شد";

            return result;
        }

        public async Task<BaseResponse> DeleteProductByProductId(int productId)
        {
            BaseResponse result = new BaseResponse();

            var product = await _productRepository.GetSingle(n => n.Id == productId);

            if (product == null)
            {
                result.Status = Status.NotFound;
                result.Message = "محصولی با این ایدی پیدا نشد";

                return result;
            }


            product.IsDelete = true;

            _productRepository.Update(product);
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
            await _categoryRepository.Insert(Category);

            await _categoryRepository.SaveChanges();

            return true;
        }

        public async Task<bool> RemoveCatagory(int Id)
        {
            var item = await _categoryRepository.GetSingle(n => n.Id == Id);
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
            EditCategory.Categories = (List<Category>)await _categoryRepository.GetAll();
            var item = await _categoryRepository.GetSingle(n => n.Id == Id);
            EditCategory.Name = item.Title;
            EditCategory.Id = Id;
            if (item.ParentId != null)
                EditCategory.CategoryId = item.ParentId;

            return EditCategory;
        }

        public async Task<bool> EditCategory(EditCategoryViewModel category)
        {
            var item = await _categoryRepository.GetSingle(n => n.Id == category.Id);
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
            await _galleryRepository.Insert(productGallery);

            await _galleryRepository.SaveChanges();

            return true;
        }

        public async Task<int> RemoveGalleryImage(int imageId)
        {
            var item = await _galleryRepository.GetSingle(n => n.Id == imageId);
            item.IsDelete = true;

            _galleryRepository.Update(item);
            await _galleryRepository.SaveChanges();

            return item.ProductId;
        }

        #endregion

        #region Colors


        public async Task<DataResponse<IReadOnlyList<ListColorViewModel>>> GetAllColors(int pageId)
        {
            DataResponse<IReadOnlyList<ListColorViewModel>> result = new DataResponse<IReadOnlyList<ListColorViewModel>>();

            List<ListColorViewModel> data = new List<ListColorViewModel>();

            var colors = await _colorRepository.GetAllColorsWithProductInclude();

            if (colors == null)
            {
                result.Status = Status.NotFound;
                result.Message = "رنگی وجود ندارد";

                return result;
            }

            int take = 10;
            int skip = (pageId - 1) * take;

            var lColors = colors.Skip(skip).Take(take).OrderByDescending(u => u.ProductId).ToList();

            foreach (var color in lColors)
            {
                data.Add(new ListColorViewModel()
                {
                    ProductName = color.Product.Name,
                    ProductId = color.ProductId,
                    IsDefault = color.IsDefault,
                    Price = color.Price,
                    Count = color.Count,
                    Name = color.Name,
                    Hex = color.Hex,
                    Id = color.Id
                });
            }

            result.Status = Status.Success;
            result.Message = "دریافت رنگ ها با موفقیت انجام شد";
            result.Data = data.AsReadOnly();

            return result;
        }

        public async Task<BaseResponse> AddColor(AddColorViewModel addColor)
        {
            BaseResponse result = new BaseResponse();

            int productId = await _productRepository.GetProductIdByName(addColor.ProductName);

            if (productId == 0)
            {
                result.Status = Status.NotFound;
                result.Message = "محصولی با این نام پیدا نشد";

                return result;
            }

            if (addColor.IsDefault)
            {
                var defColor = await _colorRepository.GetSingle(n => n.ProductId == productId);

                if (defColor != null)
                {
                    defColor.IsDefault = false;

                    _colorRepository.Update(defColor);
                    await _colorRepository.SaveChanges();
                }

            }

            Color color = new Color()
            {
                IsDefault = addColor.IsDefault,
                Count = addColor.Count,
                Price = addColor.Price,
                ProductId = productId,
                Name = addColor.Name,
                Hex = addColor.Hex,
            };

            await _colorRepository.Insert(color);
            await _colorRepository.SaveChanges();

            result.Status = Status.Success;
            result.Message = "افزودن رنگ با موفقیت انجام شد";

            return result;
        }


        public async Task<DataResponse<EditColorViewModel>> GetEditColor(int colorId)
        {
            DataResponse<EditColorViewModel> result = new DataResponse<EditColorViewModel>();
            var ressult = _colorRepository.GetSingle(c => c.Id == colorId);
            return result;

        }

        #endregion
    }
}
