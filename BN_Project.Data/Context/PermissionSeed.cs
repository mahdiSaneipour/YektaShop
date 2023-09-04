using BN_Project.Domain.Entities.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BN_Project.Data.Context
{
    public class PermissionSeed : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.HasData(new Permission
            {
                Id = 1,
                UniqeName = "Comments_Management",
                Title = "مدیریت کامنت ها"
            },
            new Permission
            {
                Id = 2,
                UniqeName = "Comments_Comments",
                Title = "کامنت ها",
                ParentId = 1
            },
            new Permission
            {
                Id = 3,
                UniqeName = "ConfirmComment_Comment",
                Title = "تایید کامنت",
                ParentId = 1
            },
            new Permission
            {
                Id = 4,
                UniqeName = "CloseComment_Comment",
                Title = "بستن کامنت",
                ParentId = 1
            },
            new Permission
            {
                Id = 5,
                UniqeName = "Tickets_Management",
                Title = "مدیریت تیکت ها"
            },
            new Permission
            {
                Id = 6,
                UniqeName = "Tickets_Tickets",
                Title = "تیکت ها",
                ParentId = 5
            },
            new Permission
            {
                Id = 7,
                UniqeName = "CloseTicket_Tickets",
                Title = "بستن تیکت",
                ParentId = 5
            },
            new Permission
            {
                Id = 8,
                UniqeName = "AddTicketMessage_Tickets",
                Title = "جزئیات تیکت",
                ParentId = 5
            },
            new Permission
            {
                Id = 9,
                UniqeName = "SendMessage_Tickets",
                Title = "پاسخ به تیکت",
                ParentId = 5
            },
            new Permission
            {
                Id = 10,
                UniqeName = "AddTicket_Tickets",
                Title = "باز کردن تیکت",
                ParentId = 5
            },
            new Permission
            {
                Id = 11,
                UniqeName = "Users_Management",
                Title = "مدیریت کاربران"
            },
            new Permission
            {
                Id = 12,
                UniqeName = "Users_Users",
                Title = "کاربران",
                ParentId = 11
            },
            new Permission
            {
                Id = 13,
                UniqeName = "AddUser_Users",
                Title = "افزودن کاربران",
                ParentId = 11
            },
            new Permission
            {
                Id = 14,
                UniqeName = "RemoveUser_Users",
                Title = "حذف کاربر",
                ParentId = 11
            },
            new Permission
            {
                Id = 15,
                UniqeName = "EditUser_Users",
                Title = "ویرایش کاربر",
                ParentId = 11
            },
            new Permission
            {
                Id = 16,
                UniqeName = "Products_Management",
                Title = "مدیریت محصولات"
            },
            new Permission
            {
                Id = 17,
                UniqeName = "Products_Products",
                Title = "محصولات",
                ParentId = 16
            },
            new Permission
            {
                Id = 18,
                UniqeName = "AddProduct_Products",
                Title = "افزودن محصول",
                ParentId = 16
            },
            new Permission
            {
                Id = 19,
                UniqeName = "EditProduct_Products",
                Title = "ویرایش محصول",
                ParentId = 16
            },
            new Permission
            {
                Id = 20,
                UniqeName = "DeleteProduct_Products",
                Title = "حذف محصول",
                ParentId = 16
            },
            new Permission
            {
                Id = 21,
                UniqeName = "Categories_Products",
                Title = "دسته بندی ها"
            },
            new Permission
            {
                Id = 22,
                UniqeName = "AddCategory_Products",
                Title = "افزودن دسته بندی",
                ParentId = 21
            },
            new Permission
            {
                Id = 23,
                UniqeName = "EditCategory_Products",
                Title = "ویرایش دسته بندی",
                ParentId = 21
            },
            new Permission
            {
                Id = 24,
                UniqeName = "RemoveCategory_Products",
                Title = "حذف دسته بندی",
                ParentId = 21
            },
            new Permission
            {
                Id = 25,
                UniqeName = "Colors_Products",
                Title = "زنگ ها"
            },
            new Permission
            {
                Id = 26,
                UniqeName = "AddColor_Priducts",
                Title = "افزودن رنگ",
                ParentId = 25
            },
            new Permission
            {
                Id = 27,
                UniqeName = "EditColor_Products",
                Title = "ویرایش رنگ",
                ParentId = 25
            },
            new Permission
            {
                Id = 28,
                UniqeName = "DeleteColor_Products",
                Title = "حذف رنگ",
                ParentId = 25
            },
            new Permission
            {
                Id = 29,
                UniqeName = "Gallery_Products",
                Title = "عکس محصولات",
            },
            new Permission
            {
                Id = 30,
                UniqeName = "AddImage_Products",
                Title = "افزودن تصویر",
                ParentId = 29
            },
            new Permission
            {
                Id = 31,
                UniqeName = "RemoveImage_Products",
                Title = "حذف تصویر",
                ParentId = 29
            },
            new Permission
            {
                Id = 32,
                UniqeName = "Discounts_Products",
                Title = "تخفیف ها"
            },
            new Permission
            {
                Id = 33,
                UniqeName = "AddDiscount_Products",
                Title = "افزودن تخفیف",
                ParentId = 32
            },
            new Permission
            {
                Id = 34,
                UniqeName = "EditDiscount_Products",
                Title = "ویرایش تخفیف",
                ParentId = 32
            },
            new Permission
            {
                Id = 35,
                UniqeName = "RemoveDiscount_Products",
                Title = "حذف تخفیف",
                ParentId = 32
            },
            new Permission
            {
                Id = 36,
                UniqeName = "Admin_Index",
                Title = "صفحه اصلی ادمین"
            });
        }
    }
}
