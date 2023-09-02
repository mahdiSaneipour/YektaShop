using BN_Project.Domain.Entities;

namespace BN_Project.Domain.IRepository
{
    public interface IDiscountRepository : IGenericRepositroy<Discount>
    {
        public Task<Discount> GetDiscountWithProducts(int Id);

        public Task<bool> IsDiscountAvailableForPublicProduct(int discountId);

        public Task<int> GetPercentByDiscountId(int discountId);

        public Task<bool> IsDiscountCodeValid(string discount);

        public Task<Discount> GetDiscountByDiscountCodeWithIncludeProducts(string discount);

        public Task<List<int>> GetPublicDiscountsPercentList();
    }
}
