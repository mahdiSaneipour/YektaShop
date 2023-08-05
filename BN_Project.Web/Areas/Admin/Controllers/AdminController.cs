using BN_Project.Core.Response.Status;
using BN_Project.Core.Services.Interfaces;
using BN_Project.Domain.ViewModel.Admin;
using Microsoft.AspNetCore.Mvc;
namespace BN_Project.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("[Controller]")]
    public class AdminController : Controller
    {
        private readonly IUserServices _userService;
        private readonly IProductServices _productService;

        public AdminController(IUserServices userService,
            IProductServices productService)
        {
            _userService = userService;
            _productService = productService;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region Users

        [Route("Users")]
        public async Task<IActionResult> Users(int pageId = 1)
        {
            var result = await _userService.GetUsersForAdmin(pageId);

            if (result.Status == Status.Success || result.Status == Status.NotFound)
            {
                return View(result.Data);
            }

            return RedirectToAction("Index");
        }

        [Route("AddUser")]
        public async Task<IActionResult> AddUser()
        {
            return View();
        }

        [HttpPost]
        [Route("Users")]
        public async Task<IActionResult> AddUser(AddUserViewModel addUser)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var result = await _userService.AddUserFromAdmin(addUser);

            switch (result.Status)
            {
                case Status.AlreadyHave:
                    ModelState.AddModelError("Email", result.Message);
                    return View();

                case Status.AlreadyHavePhoneNumber:
                    ModelState.AddModelError("PhoneNumber", result.Message);
                    return View();
            }

            if (result.Status == Status.Success)
            {
                return RedirectToAction("Users");
            }
            else
            {
                ModelState.AddModelError("Email", "خطایی در سیستم رخ داده لطفا بعدا امتحان کنید");
                return View();
            }
        }

        [Route("RemoveUser/{userId}")]
        public async Task<IActionResult> RemoveUser(int userId)
        {
            if (await _userService.RemoveUserById(userId))
            {
                return RedirectToAction("Users");
            }
            else
            {
                return NotFound();
            }
        }

        [Route("EditUser/{userId}")]
        public async Task<IActionResult> EditUser(int userId)
        {
            var item = await _userService.GetUserById(userId);
            return View(item);
        }

        [HttpPost]
        [Route("EditUser")]
        public async Task<IActionResult> EditUser(EditUserViewModel user)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var result = await _userService.EditUsers(user);

            switch (result.Status)
            {
                case Status.AlreadyHave:
                    ModelState.AddModelError("Email", result.Message);
                    return View();

                case Status.AlreadyHavePhoneNumber:
                    ModelState.AddModelError("PhoneNumber", result.Message);
                    return View();
            }

            if (result.Status == Status.Success)
            {
                return RedirectToAction("Users");
            }
            else
            {
                ModelState.AddModelError("Email", "خطایی در سیستم رخ داده لطفا بعدا امتحان کنید");
                return View();
            }
        }

        #endregion

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

            var categories = await _productService.GetParentCategories();
            ViewData["Categories"] = categories.Item1;

            var id = categories.Item1.FirstOrDefault();
            ViewData["SubCategories"] = await _productService.GetSubCategories(int.Parse(id.Value));

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

            if (result.Status != Status.Success)
            {
                var categories = await _productService.GetParentCategories();
                ViewData["Categories"] = categories.Item1;

                ViewData["SubCategories"] = await _productService.GetSubCategories(int.Parse(categories.Item1.FirstOrDefault().Value));

                return View();
            }

            return RedirectToAction("Products");
        }

        [Route("EditProduct/{productId}")]
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

            var categories = await _productService.GetParentCategories(model.CategoryId);
            ViewData["Categories"] = categories.Item1;

            ViewData["SubCategories"] = await _productService.GetSubCategories((int)categories.Item2, model.CategoryId);

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

            var categories = await _productService.GetParentCategories(editProduct.CategoryId);
            ViewData["Categories"] = categories.Item1;

            ViewData["SubCategories"] = await _productService.GetSubCategories((int)categories.Item2, editProduct.CategoryId);

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

        [HttpPost]
        [Route("AddCategory")]
        public async Task<IActionResult> AddCategory(AddCategoriesViewModel category)
        {
            await _productService.AddCategory(category);

            return RedirectToAction("Categories", "Admin");
        }

        [Route("RemoveCategory")]
        public async Task<IActionResult> RemoveCategory(int categoryId)
        {
            if (categoryId == 0)
                return NotFound();
            if (await _productService.RemoveCatagory(categoryId))
                return RedirectToAction("Categories", "Admin");
            return NotFound();
        }

        [Route("EditCategory/{categoryId}")]
        public async Task<IActionResult> EditCategory(int categoryId)
        {
            var item = await _productService.GetCategoryById(categoryId);
            return View(item);
        }

        [HttpPost]
        [Route("EditCategory")]
        public async Task<IActionResult> EditCategory(EditCategoryViewModel category)
        {
            await _productService.EditCategory(category);
            return RedirectToAction("Categories", "Admin");
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

            return RedirectToAction("Index");
        }

        [Route("AddColor")]
        public async Task<IActionResult> AddColor()
        {
            var ready = await _productService.ColorReadyForAddAndEdit();

            if (ready.Status == Status.Success)
            {
                return View();
            }

            return RedirectToAction("Products");
        }

        [HttpPost]
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

        [Route("EditColor/{colorId}")]
        public async Task<IActionResult> EditColor(int colorId)
        {
            var ready = await _productService.ColorReadyForAddAndEdit();

            if (ready.Status == Status.NotFound)
            {
                return RedirectToAction("Products");
            }

            var result = await _productService.GetEditColor(colorId);

            if (result.Status == Status.Success)
            {
                return View(result.Data);
            }

            return RedirectToAction("Colors");
        }

        [HttpPost]
        [Route("EditColor")]
        public async Task<IActionResult> EditColor(EditColorViewModel color)
        {
            if(!ModelState.IsValid)
            {
                var data = await _productService.GetEditColor(color.ColorId);
                return View(data.Data);
            }

            var ready = await _productService.ColorReadyForAddAndEdit();

            if (ready.Status == Status.NotFound)
            {
                return RedirectToAction("Products");
            }

            var result = await _productService.EditColor(color);

            if (result.Status != Status.Success)
            {
                var data = await _productService.GetEditColor(color.ColorId);
                return View(data.Data);

            } else if (result.Status == Status.NotFound)
            {
                ModelState.AddModelError("ProductName", result.Message);

                var data = await _productService.GetEditColor(color.ColorId);
                return View(data.Data);
            }

            return RedirectToAction("Colors");
        }

        [Route("DeleteColorById/{colorId}")]
        public async Task<IActionResult> DeleteColorById(int colorId)
        {
            await _productService.DeleteColorById(colorId);
            return RedirectToAction("Colors");
        }

        #endregion

        #region Galleries

        [Route("Gallery/{Id}")]
        public async Task<IActionResult> Gallery(int Id)
        {
            if (Id == 0)
                return NotFound();
            var gallery = await _productService.GetGalleryByProductId(Id);
            gallery.PriductId = Id;

            return View(gallery);
        }

        [Route("AddImage/{Id}")]
        public IActionResult AddImage(int Id)
        {
            AddGalleryViewModel addGallery = new AddGalleryViewModel()
            {
                ProductId = Id
            };
            return View(addGallery);
        }

        [HttpPost]
        [Route("AddImage")]
        public async Task<IActionResult> AddImage(AddGalleryViewModel gallery)
        {
            if (!ModelState.IsValid)
                return View();
            if (!await _productService.AddGalleryImage(gallery))
                return View();

            return RedirectToAction("Gallery", "Admin", new { Id = gallery.ProductId });
        }

        [Route("RemoveImage/{Id}")]
        public async Task<IActionResult> RemoveImage(int Id)
        {
            if (Id == 0)
                return NotFound();
            int ProductId = await _productService.RemoveGalleryImage(Id);

            return RedirectToAction("Gallery", "Admin", new { Id = ProductId });
        }
        #endregion
    }
}