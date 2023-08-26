using BN_Project.Domain.Entities;

namespace BN_Project.Domain.IRepository
{
    public interface IDiscountRepository : IGenericRepositroy<Discount>
    {
        public Task<Discount> GetDiscountWithProducts(int Id);

        public Task<bool> IsDiscountAvailable(int discountId);

        public Task<int> GetPercentByDiscountId(int discountId);
    }
}
