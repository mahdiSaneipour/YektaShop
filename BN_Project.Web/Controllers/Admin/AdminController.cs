using BN_Project.Core.IService.Admin;
using BN_Project.Core.Response.Status;
using BN_Project.Domain.ViewModel.Admin;
using Microsoft.AspNetCore.Mvc;

namespace BN_Project.Web.Controllers.Admin
{
    public class AdminController : Controller
    {
        private readonly IAdminServices _adminServices;

        public AdminController(IAdminServices adminServices)
        {
            _adminServices = adminServices;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region Users

        public async Task<IActionResult> Users(int pageId = 1)
        {
            var result = await _adminServices.GetUsersForAdmin(pageId);

            if(result.Status == Status.Success || result.Status == Status.NotFound)
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

            var result = await _adminServices.AddUserFromAdmin(addUser);

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
            if (await _adminServices.RemoveUserById(Id))
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
            return View();
        }

        #endregion

        #region Products

        public async Task<IActionResult> Products()
        {
            var result = await _adminServices.GetProducts();

            if (result.Status == Status.Success || result.Status == Status.NotFound)
            {
                return View(result.Data);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> AddProduct()
        {
            var categories = await _adminServices.GetParentCategories();
            ViewData["Categories"] = categories.Item1;

            ViewData["SubCategories"] = await _adminServices.GetSubCategories(Int32.Parse(categories.Item1.FirstOrDefault().Value));
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(AddProductViewModel addProduct)
        {
            if(!ModelState.IsValid)
            {
                var categories = await _adminServices.GetParentCategories();
                ViewData["Categories"] = categories.Item1;

                ViewData["SubCategories"] = await _adminServices.GetSubCategories(Int32.Parse(categories.Item1.FirstOrDefault().Value));

                return View();
            }

            var result = await _adminServices.AddProduct(addProduct);

            if(result.Status != Status.Success)
            {
                var categories = await _adminServices.GetParentCategories();
                ViewData["Categories"] = categories.Item1;

                ViewData["SubCategories"] = await _adminServices.GetSubCategories(Int32.Parse(categories.Item1.FirstOrDefault().Value));

                return View();
            }

            return RedirectToAction("Products");
        }

        public async Task<IActionResult> EditProduct(int productId)
        {
            if(!ModelState.IsValid) {
                return View();
            }

            EditProductViewModel model = new EditProductViewModel();

            var result = await _adminServices.GetProductForEdit(productId);

            if (result.Status == Status.Success)
            {
                model = result.Data;
            }  else
            {
                return RedirectToAction("Products");
            }

            var categories = await _adminServices.GetParentCategories(model.CategoryId);
            ViewData["Categories"] = categories.Item1;

            ViewData["SubCategories"] = await _adminServices.GetSubCategories((int) categories.Item2, model.CategoryId);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(EditProductViewModel editProduct)
        {

            var result = await _adminServices.EditProduct(editProduct);

            if(result.Status == Status.Success)
            {
                return RedirectToAction("Products");
            }

            var categories = await _adminServices.GetParentCategories(editProduct.CategoryId);
            ViewData["Categories"] = categories.Item1;

            ViewData["SubCategories"] = await _adminServices.GetSubCategories((int)categories.Item2, editProduct.CategoryId);

            return View(editProduct);
        }

        #endregion
    }
}
