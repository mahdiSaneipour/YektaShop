using BN_Project.Core.Response.Status;
using BN_Project.Core.Services.Interfaces;
using BN_Project.Core.Tools;
using BN_Project.Domain.ViewModel.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BN_Project.Web.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("[Controller]")]
    public class ProductsController : Controller
    {
        private readonly IProductServices _productService;
        public ProductsController(IProductServices productService)
        {
            _productService = productService;
        }

        [Route("Admin")]
        public IActionResult Index()
        {
            return View();
        }

        #region Products

        [Route("Products")]
        public async Task<IActionResult> Products()
        {
            var result = await _productService.GetProducts();

            if (result.Status == Status.Success || result.Status == Status.NotFound)
            {
                return View(result.Data);
            }

            return RedirectToAction("Index");
        }

        [Route("AddProduct")]
        public async Task<IActionResult> AddProduct()
        {
            var ready = await _productService.ProductReadyForAddAndEdit();

            if (ready.Status == Status.NotFound)
            {
                return RedirectToAction("Categories");
            }

            return View();
        }

        [HttpPost]
        [Route("AddProduct")]
        public async Task<IActionResult> AddProduct(AddProductViewModel addProduct)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _productService.GetParentCategories();
                ViewData["Categories"] = categories.Item1;

                ViewData["SubCategories"] = await _productService.GetSubCategories(int.Parse(categories.Item1.FirstOrDefault().Value));

                return View();
            }

            var result = await _productService.AddProduct(addProduct);

            if (result.Status == Status.AlreadyHave)
            {
                ModelState.AddModelError("Title", result.Message);

                var categories = await _productService.GetParentCategories();
                ViewData["Categories"] = categories.Item1;

                ViewData["SubCategories"] = await _productService.GetSubCategories(int.Parse(categories.Item1.FirstOrDefault().Value));

                return View();
            }
            else if (result.Status != Status.Success)
            {
                var categories = await _productService.GetParentCategories();
                ViewData["Categories"] = categories.Item1;

                ViewData["SubCategories"] = await _productService.GetSubCategories(int.Parse(categories.Item1.FirstOrDefault().Value));

                return View();
            }

            return RedirectToAction("Products");
        }

        [Route("EditProduct")]
        public async Task<IActionResult> EditProduct(int productId)
        {
            var ready = await _productService.ProductReadyForAddAndEdit();

            if (ready.Status == Status.NotFound)
            {
                return RedirectToAction("Categories");

            }

            EditProductViewModel model = new EditProductViewModel();

            var result = await _productService.GetProductForEdit(productId);

            if (result.Status == Status.Success)
            {
                model = result.Data;
            }
            else
            {
                return RedirectToAction("Products");
            }

            int categoryId = await _productService.GetCategoryIdByCategoryName(model.Category);

            return View(model);
        }

        [HttpPost]
        [Route("EditProduct")]
        public async Task<IActionResult> EditProduct(EditProductViewModel editProduct)
        {
            var result = await _productService.EditProduct(editProduct);

            if (result.Status == Status.Success)
            {
                return RedirectToAction("Products");
            }
            else if (result.Status == Status.AlreadyHave)
            {
                ModelState.AddModelError("Title", result.Message);

                return View(editProduct);
            }

            return View(editProduct);
        }


        [Route("DeleteProduct/{productId}")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            await _productService.DeleteProductByProductId(productId);

            return RedirectToAction("Products");
        }

        #endregion

        #region Categories

        [Route("Categories")]
        public async Task<IActionResult> Categories()
        {
            var items = await _productService.GetAllCategories();
            return View(items);
        }

        [Route("AddCategory")]
        public async Task<IActionResult> AddCategory()
        {
            AddCategoriesViewModel AddCategory = new AddCategoriesViewModel()
            {
                Categories = await _productService.GetAllCategories()
            };
            return View(AddCategory);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Route("AddCategory")]
        public async Task<IActionResult> AddCategory(AddCategoriesViewModel category)
        {
            var result = await _productService.AddCategory(category);

            if (result)
            {
                return RedirectToAction(nameof(Categories));
            }
            else
            {
                ModelState.AddModelError("Name", "دسته بندی با این نام وجود دارد");

                AddCategoriesViewModel AddCategory = new AddCategoriesViewModel()
                {
                    Categories = await _productService.GetAllCategories()
                };
                return View(AddCategory);
            }
        }

        [Route("RemoveCategory")]
        public async Task<IActionResult> RemoveCategory(int categoryId)
        {
            if (categoryId == 0)
                return NotFound();
            if (await _productService.RemoveCatagory(categoryId))
                return RedirectToAction(nameof(Categories));
            return NotFound();
        }

        [Route("EditCategory")]
        public async Task<IActionResult> EditCategory(int categoryId)
        {
            var item = await _productService.GetCategoryById(categoryId);
          
            return View(item);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Route("EditCategory")]
        public async Task<IActionResult> EditCategory(EditCategoryViewModel category)
        {
            var result = await _productService.EditCategory(category);

            if (result)
            {
                return RedirectToAction(nameof(Categories));
            }
            else
            {
                ModelState.AddModelError("Name", "دسته بندی با این نام وجود دارد");
                var item = await _productService.GetCategoryById(category.Id);
                return View(item);
            }
        }

        #endregion

        #region Colors

        [Route("Colors")]
        public async Task<IActionResult> Colors(int pageId = 1)
        {
            var result = await _productService.GetAllColors(pageId);

            if (result.Status == Status.Success || result.Status == Status.NotFound)
            {
                return View(result.Data);
            }

            return RedirectToAction(nameof(Index));
        }

        [Route("AddColor")]
        public async Task<IActionResult> AddColor()
        {
            var ready = await _productService.ColorReadyForAddAndEdit();

            if (ready.Status == Status.Success)
            {
                return View();
            }

            return RedirectToAction(nameof(Products));
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Route("AddColor")]
        public async Task<IActionResult> AddColor(AddColorViewModel addColor)
        {
            var result = await _productService.AddColor(addColor);

            if (result.Status == Status.Success)
            {
                return RedirectToAction("Colors");
            }
            else if (result.Status == Status.NotFound)
            {
                ModelState.AddModelError("ProductName", result.Message);
            }

            return View();
        }

        [Route("EditColor")]
        public async Task<IActionResult> EditColor(int colorId)
        {
            var ready = await _productService.ColorReadyForAddAndEdit();

            if (ready.Status == Status.NotFound)
            {
                return RedirectToAction(nameof(Products));
            }

            var result = await _productService.GetEditColor(colorId);

            if (result.Status == Status.Success)
            {
                return View(result.Data);
            }

            return RedirectToAction(nameof(Colors));
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Route("EditColor")]
        public async Task<IActionResult> EditColor(EditColorViewModel color)
        {
            if (!ModelState.IsValid)
            {
                var data = await _productService.GetEditColor(color.ColorId);
                return View(data.Data);
            }

            var ready = await _productService.ColorReadyForAddAndEdit();

            if (ready.Status == Status.NotFound)
            {
                return RedirectToAction(nameof(Products));
            }

            var result = await _productService.EditColor(color);

            if (result.Status != Status.Success)
            {
                var data = await _productService.GetEditColor(color.ColorId);
                return View(data.Data);

            }
            else if (result.Status == Status.NotFound)
            {
                ModelState.AddModelError("ProductName", result.Message);

                var data = await _productService.GetEditColor(color.ColorId);
                return View(data.Data);
            }

            return RedirectToAction(nameof(Colors));
        }

        [Route("DeleteColorById")]
        public async Task<IActionResult> DeleteColorById(int colorId)
        {
            await _productService.DeleteColorById(colorId);
            return RedirectToAction(nameof(Colors));
        }

        #endregion

        #region Galleries

        [Route("Gallery")]
        public async Task<IActionResult> Gallery(int Id)
        {
            if (Id == 0)
                return NotFound();
            var gallery = await _productService.GetGalleryByProductId(Id);
            gallery.PriductId = Id;

            return View(gallery);
        }

        [Route("AddImage")]
        public IActionResult AddImage(int imageId)
        {
            AddGalleryViewModel addGallery = new AddGalleryViewModel()
            {
                ProductId = imageId
            };
            return View(addGallery);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Route("AddImage")]
        public async Task<IActionResult> AddImage(AddGalleryViewModel gallery)
        {
            if (!ModelState.IsValid)
                return View();
            if (!await _productService.AddGalleryImage(gallery))
                return View();

            return RedirectToAction(nameof(Gallery), new { Id = gallery.ProductId });
        }

        [Route("RemoveImage")]
        public async Task<IActionResult> RemoveImage(int Id)
        {
            if (Id == 0)
                return NotFound();
            int ProductId = await _productService.RemoveGalleryImage(Id);

            return RedirectToAction(nameof(Gallery), new { Id = ProductId });
        }
        #endregion

        #region Discount

        [Route("Discounts")]
        public async Task<IActionResult> Discounts()
        {
            var items = await _productService.GetAllDiscounts();
            return View(items);
        }

        [Route("AddDiscount")]
        public async Task<IActionResult> AddDiscount()
        {
            AddDiscountViewModel AddDiscount = new AddDiscountViewModel();
            AddDiscount.Products = await _productService.GetAllProductsForDiscount();
            return View(AddDiscount);
        }

        [HttpPost]
        [Route("AddDiscount")]
        public async Task<IActionResult> AddDiscount(AddDiscountViewModel discount)
        {
            discount.StartDate = Convert.ToDateTime(discount.StartDate).ToMiladi();
            discount.ExpireDate = Convert.ToDateTime(discount.ExpireDate).ToMiladi();

            if (await _productService.AddDiscount(discount))
            {
                return RedirectToAction(nameof(Discounts));
            }
            else
            {
                ModelState.AddModelError("Percent", "خطایی رخ داد لطفا دوباره امتحان کنید!");
                return View(discount);
            }
        }

        [Route("RemoveDiscount")]
        public async Task<IActionResult> RemoveDiscount(int Id)
        {
            if (Id == 0)
                return NotFound();
            if (await _productService.RemoveDiscount(Id))
            {
                return RedirectToAction(nameof(Discounts));
            }
            else
            {
                return NotFound();
            }
        }

        [Route("ViewProductsForDiscount")]
        public async Task<IActionResult> ViewProductsForDiscount(int Id)
        {
            if (Id == 0)
                return NotFound();

            var products = await _productService.GetListOfProducts(Id);

            return View("ReturnProductsForDiscount", products);
        }

        [Route("EditDiscount")]
        public async Task<IActionResult> EditDiscount(int Id)
        {
            if (Id == 0)
                return NotFound();
            var item = await _productService.GetDiscountForEdit(Id);
            item.Products = await _productService.GetAllProductsForDiscount();
            return View(item);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Route("EditDiscount")]
        public async Task<IActionResult> EditDiscount(EditDiscountViewModel editDiscount)
        {
            editDiscount.StartDate = Convert.ToDateTime(editDiscount.StartDate).ToMiladi();
            editDiscount.ExpireDate = Convert.ToDateTime(editDiscount.ExpireDate).ToMiladi();
            if (await _productService.EditDiscount(editDiscount))
            {
                return RedirectToAction(nameof(Discounts));
            }
            else
            {
                ModelState.AddModelError("Percent", "خطایی رخ داد لطفا دوباره امتحان کنید!");
                return View(editDiscount);
            }
        }

        #endregion
    }
}
