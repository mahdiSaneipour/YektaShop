using BN_Project.Core.Response.Status;
using BN_Project.Core.Services.Interfaces;
using BN_Project.Domain.ViewModel.Admin;
using Microsoft.AspNetCore.Mvc;
namespace BN_Project.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
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

        public async Task<IActionResult> Users(int pageId = 1)
        {
            var result = await _userService.GetUsersForAdmin(pageId);

            if (result.Status == Status.Success || result.Status == Status.NotFound)
            {
                return View(result.Data);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> AddUser()
        {
            return View();
        }

        [HttpPost]
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

        public async Task<IActionResult> RemoveUser(int Id)
        {
            if (await _userService.RemoveUserById(Id))
            {
                return RedirectToAction("Users", "Admin");
            }
            else
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> EditUser(int Id)
        {
            var item = await _userService.GetUserById(Id);
            return View("~/Views/Admin/EditUser.cshtml", item);
        }

        [HttpPost]
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

        public async Task<IActionResult> Products()
        {
            var result = await _productService.GetProducts();

            if (result.Status == Status.Success || result.Status == Status.NotFound)
            {
                return View(result.Data);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> AddProduct()
        {
            var categories = await _productService.GetParentCategories();
            ViewData["Categories"] = categories.Item1;

            var id = categories.Item1.FirstOrDefault();
            ViewData["SubCategories"] = await _productService.GetSubCategories(int.Parse(id.Value));

            return View();
        }

        [HttpPost]
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

        public async Task<IActionResult> EditProduct(int productId)
        {

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

        #endregion

        #region Categories

        public async Task<IActionResult> Categories()
        {
            var items = await _productService.GetAllCategories();
            return View(items);
        }

        public async Task<IActionResult> AddCategory()
        {
            AddCategoriesViewModel AddCategory = new AddCategoriesViewModel()
            {
                Categories = await _productService.GetAllCategories()
            };
            return View(AddCategory);
        }

        public async Task<IActionResult> RemoveCategory(int Id)
        {
            if (Id == 0)
                return NotFound();
            if (await _productService.RemoveCatagory(Id))
                return RedirectToAction("Categories", "Admin");
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(AddCategoriesViewModel category)
        {
            await _productService.AddCategory(category);

            return RedirectToAction("Categories", "Admin");
        }

        public async Task<IActionResult> EditCategory(int Id)
        {
            var item = await _productService.GetCategoryById(Id);
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> EditCategory(EditCategoryViewModel category)
        {
            await _productService.EditCategory(category);
            return RedirectToAction("Categories", "Admin");
        }

        #endregion

        #region Colors

        public async Task<IActionResult> Colors(int pageId = 1)
        {
            var result = await _productService.GetAllColors(pageId);

            if (result.Status == Status.Success || result.Status == Status.NotFound)
            {
                return View(result.Data);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> AddColor()
        {

            return View();
        }

        [HttpPost]
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

        public async Task<IActionResult> EditColor(int colorId)
        {
            var result = await _productService.GetEditColor(colorId);

            return View();
        }

        #endregion

        #region Galleries
        public async Task<IActionResult> Gallery(int Id)
        {
            if (Id == 0)
                return NotFound();
            var gallery = await _productService.GetGalleryByProductId(Id);
            gallery.PriductId = Id;

            return View(gallery);
        }

        public IActionResult AddImage(int Id)
        {
            AddGalleryViewModel addGallery = new AddGalleryViewModel()
            {
                ProductId = Id
            };
            return View(addGallery);
        }

        [HttpPost]
        public async Task<IActionResult> AddImage(AddGalleryViewModel gallery)
        {
            if (!ModelState.IsValid)
                return View();
            if (!await _productService.AddGalleryImage(gallery))
                return View();

            return RedirectToAction("Gallery", "Admin", new { Id = gallery.ProductId });
        }

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