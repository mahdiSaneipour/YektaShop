using BN_Project.Core.Response;
using BN_Project.Core.Response.DataResponse;
using BN_Project.Core.Response.Status;
using BN_Project.Core.Services.Interfaces;
using BN_Project.Domain.Entities;
using BN_Project.Domain.IRepository;
using BN_Project.Domain.ViewModel.Admin;
using BN_Project.Domain.ViewModel.Product;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Runtime.CompilerServices;

namespace BN_Project.Core.Services.Implementations
{
    public class ProductServices : IProductServices
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IColorRepository _colorRepository;
        private readonly IGalleryRepository _galleryRepository;
        private readonly IDiscountRepository _discountRepository;
        private readonly IDiscountProductRepository _discountProductRepository;

        public ProductServices(IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IGalleryRepository galleryRepository,
            IColorRepository colorRepository,
            IDiscountRepository discountRepository,
            IDiscountProductRepository discountProductRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _galleryRepository = galleryRepository;
            _colorRepository = colorRepository;
            _discountRepository = discountRepository;
            _discountProductRepository = discountProductRepository;
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

            if (await _productRepository.IsProductNameExist(addProduct.Title))
            {
                result.Status = Status.AlreadyHave;
                result.Message = "محصولی با این نام موجود است";

                return result;
            }

            Product product = new Product()
            {
                Description = addProduct.Description,
                CategoryId = await _categoryRepository.GetCategoryIdByName(addProduct.Category),
                Features = addProduct.Feature,
                Price = addProduct.Price,
                Image = addProduct.Image,
                Name = addProduct.Title
            };

            await _productRepository.Insert(product);
            await _productRepository.SaveChanges();

            result.Status = Status.Success;
            result.Message = "محصول با موفقیت افزوده شد";

            return result;
        }

        public async Task<DataResponse<EditProductViewModel>> GetProductForEdit(int productId)
        {
            DataResponse<EditProductViewModel> result = new DataResponse<EditProductViewModel>();

            var product = await _productRepository.GetProductByIdWithIncludeCategory(productId);

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
                Category = product.Category.Title,
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

        public async Task<DataResponse<List<ListProductViewModel>>> GetProductsListShowByCategoryId(int categoryId)
        {
            DataResponse<List<ListProductViewModel>> result = new DataResponse<List<ListProductViewModel>>();

            var products = await _productRepository.GetProductsIncludeColorsByCategoryId(categoryId);

            if (products != null)
            {
                List<ListProductViewModel> data = new List<ListProductViewModel>();

                foreach (var product in products)
                {
                    data.Add(new ListProductViewModel
                    {
                        Colors = await _colorRepository.GetHexColorsByProductId(product.Id),
                        ProductId = product.Id,
                        Price = product.Price,
                        Image = product.Image,
                        Title = product.Name,
                    });
                }

                result.Data = data;
                result.Status = Status.Success;
                result.Message = "محصوالات با موفقیت پیدا شدند";
            }
            else
            {
                result.Status = Status.NotFound;
                result.Message = "هیچ محصولی پیدا نشد";
            }

            return result;
        }

        public async Task<DataResponse<ShowProductViewModel>> GetProductForShowByProductId(int productId)
        {
            DataResponse<ShowProductViewModel> result = new DataResponse<ShowProductViewModel>();

            var product = await _productRepository.GetProductByIdWithIncludeCategoryAndColorAndImage(productId);

            List<Category> categories = new List<Category>();

            categories.Add(product.Category);

            if(product.Category.ParentCategory != null)
            {
                categories.Add(product.Category.ParentCategory);
                if(product.Category.ParentCategory.ParentCategory != null)
                {
                    categories.Add(product.Category.ParentCategory.ParentCategory);
                }
            }

            categories.Reverse();

            if (product == null)
            {
                result.Status = Status.NotFound;
                result.Message = "محصولی با این ایدی پیدا نشد";
            } else
            {

                ShowProductViewModel data = new ShowProductViewModel()
                {
                    Description = product.Description,
                    Features = product.Features,
                    Image = product.Image,
                    Title = product.Name,
                    Price = product.Price,
                    Categories = categories,
                    ProductId = product.Id,
                    Colors = product.Colors.ToList(),
                    Count = product.Colors.Sum(c => c.Count),
                    Images = product.Images.Select(c => c.ImageName).ToList(),
                    
                };

                result.Status = Status.Success;
                result.Message = "محصول با موفقیت پیداشد";
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

            if (await _productRepository.IsProductNameExist(editProduct.Title, editProduct.Id))
            {
                result.Status = Status.AlreadyHave;
                result.Message = "محصولی با این نام موجود است";

                return result;
            }


            if (product.Image != editProduct.Image)
            {
                Tools.Image.UploadImage.DeleteFile(Directory.GetCurrentDirectory() + "/wwwroot/images/products/thumb/" + product.Image);
                Tools.Image.UploadImage.DeleteFile(Directory.GetCurrentDirectory() + "/wwwroot/images/products/normal/" + product.Image);
            }

            product.Description = editProduct.Description;
            product.CategoryId = await _categoryRepository.GetCategoryIdByName(editProduct.Category);
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
            if (await _categoryRepository.IsCategoryNameExist(category.Name))
            {
                return false;
            }

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
            if (await _categoryRepository.IsCategoryNameExist(category.Name))
            {
                return false;
            }

            var item = await _categoryRepository.GetSingle(n => n.Id == category.Id);
            item.Title = category.Name;
            item.ParentId = category.CategoryId;

            _categoryRepository.Update(item);
            await _categoryRepository.SaveChanges();

            return true;

        }

        public async Task<List<Category>> GetAllCategoriesForHeader()
        {
            List<Category> categories = await _categoryRepository.GetAllCategoryWithIncludeProducts();

            return categories;
        }

        public async Task<DataResponse<List<string>>> SearchCategoriesByName(string name)
        {
            DataResponse<List<string>> result = new DataResponse<List<string>>();
            List<string> data = await _categoryRepository.SearchCategoriesByName(name);

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

        public async Task<int> GetCategoryIdByCategoryName(string name)
        {
            return await _categoryRepository.GetCategoryIdByName(name);
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

            var color = await _colorRepository.GetColorWithProductInclude(colorId);

            if (color == null)
            {
                result.Status = Status.NotFound;
                result.Message = "رنگی با این ایدی پیدا نشد";

                return result;
            }

            EditColorViewModel model = new EditColorViewModel()
            {
                IsDefault = color.IsDefault,
                Count = color.Count,
                ColorId = color.Id,
                Hex = color.Hex,
                Name = color.Name,
                Price = color.Price,
                ProductName = color.Product.Name
            };

            result.Status = Status.Success;
            result.Message = "رنگ با موفقیت پیدا شد";
            result.Data = model;

            return result;
        }

        public async Task<BaseResponse> ColorReadyForAddAndEdit()
        {
            BaseResponse result = new BaseResponse();

            if (await _productRepository.IsThereAny())
            {
                result.Status = Status.Success;
                result.Message = "محصول وجود دارد";

                return result;
            }

            result.Status = Status.NotFound;
            result.Message = "محصول وجود دارد";

            return result;
        }

        public async Task<BaseResponse> ProductReadyForAddAndEdit()
        {
            BaseResponse result = new BaseResponse();

            if (await _categoryRepository.IsThereAny())
            {
                result.Status = Status.Success;
                result.Message = "دسته بندی وجود دارد";

                return result;
            }

            result.Status = Status.NotFound;
            result.Message = "دسته بندی وجود دارد";

            return result;
        }

        public async Task<BaseResponse> EditColor(EditColorViewModel editColor)
        {
            BaseResponse result = new BaseResponse();

            int productId = await _productRepository.GetProductIdByName(editColor.ProductName);

            if (productId == 0)
            {
                result.Status = Status.NotFound;
                result.Message = "محصولی با این نام پیدا نشد";

                return result;
            }

            if (editColor.IsDefault)
            {
                var defColor = await _colorRepository.GetSingle(c => c.ProductId == productId && c.Id != editColor.ColorId);

                if (defColor != null)
                {
                    defColor.IsDefault = false;

                    _colorRepository.Update(defColor);
                    await _colorRepository.SaveChanges();
                }

            }

            Color color = new Color()
            {
                IsDefault = editColor.IsDefault,
                Count = editColor.Count,
                Price = editColor.Price,
                Id = editColor.ColorId,
                ProductId = productId,
                Name = editColor.Name,
                Hex = editColor.Hex
            };

            _colorRepository.Update(color);
            await _colorRepository.SaveChanges();

            result.Status = Status.Success;
            result.Message = "ویرایش رنگ با موفقیت انجام شد";

            return result;
        }

        public async Task<BaseResponse> DeleteColorById(int colorId)
        {
            BaseResponse result = new BaseResponse();

            var color = await _colorRepository.GetSingle(n => n.Id == colorId);

            if (color == null)
            {
                result.Status = Status.NotFound;
                result.Message = "محصولی با این ایدی پیدا نشد";

                return result;
            }

            color.IsDelete = true;

            _colorRepository.Update(color);
            await _colorRepository.SaveChanges();

            result.Status = Status.Success;
            result.Message = "رنگ با موفقیت حذف شد";

            return result;
        }

        public async Task<long> GetPriceByColorId(int colorId)
        {
            return await _colorRepository.GetColorPriceByColorId(colorId);
        }

        public async Task<long> GetCountByColorId(int colorId)
        {
            return await _colorRepository.GetColorCountByColorId(colorId);
        }
        #endregion

        #region Discount 

        public async Task<List<DiscountViewModel>> GetAllDiscounts()
        {
            var items = await _discountRepository.GetAll();
            List<DiscountViewModel> discounts = new List<DiscountViewModel>();

            //discounts.AddRange(items.Select(b => new DiscountViewModel
            //{

            //}).ToList());

            foreach (var item in items)
            {
                DiscountViewModel discount = new DiscountViewModel()
                {
                    Id = item.Id,
                    Code = item.Code,
                    StartDate = item.StartDate,
                    ExpireDate = item.ExpireDate,
                    Percent = item.Percent
                };
                discounts.Add(discount);
            }


            return discounts;
        }

        public async Task<List<ProductsForDiscountViewModel>> GetAllProductsForDiscount()
        {
            List<ProductsForDiscountViewModel> products = new List<ProductsForDiscountViewModel>();
            var items = await _productRepository.GetAll();
            foreach (var item in items)
            {
                ProductsForDiscountViewModel product = new ProductsForDiscountViewModel()
                {
                    Id = item.Id,
                    Name = item.Name
                };

                products.Add(product);
            }
            return products;
        }

        public async Task<bool> AddDiscount(AddDiscountViewModel discount)
        {
            if (discount.Percent < 0)
                return false;
            Discount item = new Discount()
            {
                Code = discount.Code,
                StartDate = discount.StartDate,
                ExpireDate = discount.ExpireDate,
                Percent = discount.Percent,
                DiscountProduct = discount.SelecetedGoods?.Select(b => new DiscountProduct
                {
                    ProductsId = b
                }).ToList()
            };

            await _discountRepository.Insert(item);
            await _discountRepository.SaveChanges();

            return true;
        }

        public async Task<bool> RemoveDiscount(int Id)
        {
            var discount = await _discountRepository.GetDiscountWithProducts(Id);
            discount.IsDelete = true;

            foreach (var item in discount.DiscountProduct)
            {
                item.IsDelete = true;
            }

            _discountRepository.Update(discount);
            await _discountRepository.SaveChanges();

            return true;
        }

        public async Task<List<ProductsForDiscountViewModel>> GetListOfProducts(int Id)
        {
            List<ProductsForDiscountViewModel> products = new List<ProductsForDiscountViewModel>();

            var discount = await _discountRepository.GetDiscountWithProducts(Id);

            foreach (var item in discount.DiscountProduct)
            {
                ProductsForDiscountViewModel product = new ProductsForDiscountViewModel()
                {
                    Id = item.Product.Id,
                    Name = item.Product.Name
                };
                products.Add(product);
            }

            return products;
        }

        public async Task<EditDiscountViewModel> GetDiscountForEdit(int Id)
        {
            var discount = await _discountRepository.GetDiscountWithProducts(Id);
            EditDiscountViewModel editDiscount = new EditDiscountViewModel()
            {
                Code = discount.Code,
                StartDate = discount.StartDate,
                ExpireDate = discount.ExpireDate,
                Percent = discount.Percent,
                Id = discount.Id,
                SelecetedGoods = discount.DiscountProduct.Select(n => n.ProductsId).ToList()
            };

            return editDiscount;
        }

        public async Task<bool> EditDiscount(EditDiscountViewModel editDiscount)
        {
            if (editDiscount.Percent != 0 && editDiscount.ExpireDate < editDiscount.StartDate)
                return false;

            var discount = await _discountRepository.GetDiscountWithProducts(editDiscount.Id);
            discount.Code = editDiscount.Code;
            discount.StartDate = editDiscount.StartDate;
            discount.ExpireDate = editDiscount.ExpireDate;
            discount.Percent = editDiscount.Percent;

            foreach (var item in discount.DiscountProduct)
            {
                item.IsDelete = true;
            }
            if (editDiscount.SelecetedGoods != null && editDiscount.SelecetedGoods.Count() != 0)
                foreach (var item in editDiscount.SelecetedGoods)
                {
                    discount.DiscountProduct.Add(new DiscountProduct
                    {
                        ProductsId = item,
                        DiscountsId = discount.Id
                    });
                }

            _discountRepository.Update(discount);
            await _discountRepository.SaveChanges();

            return true;
        }
        #endregion
    }
}