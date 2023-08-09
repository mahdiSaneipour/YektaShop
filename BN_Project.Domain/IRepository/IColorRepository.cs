using BN_Project.Domain.Entities;

namespace BN_Project.Domain.IRepository
{
    public interface IColorRepository : IGenericRepositroy<Color>
    {
        public Task<IEnumerable<Color>> GetAllColorsWithProductInclude();

        public Task<Color> GetColorWithProductInclude(int colorId);

        public Task<List<string>> GetHexColorsByProductId(int productId);

        public Task<long> GetColorPriceByColorId(int colorId);

        public Task<int> GetColorCountByColorId(int colorId);
    }
}
