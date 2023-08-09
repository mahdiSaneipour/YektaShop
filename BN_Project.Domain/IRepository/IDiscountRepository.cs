using BN_Project.Domain.Entities;

namespace BN_Project.Domain.IRepository
{
    public interface IDiscountRepository : IGenericRepositroy<Discount>
    {
        public Task<Discount> GetDiscountWithProducts(int Id);
    }
}
