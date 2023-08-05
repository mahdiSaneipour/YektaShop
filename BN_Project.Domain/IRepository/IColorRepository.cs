using BN_Project.Domain.Entities;

namespace BN_Project.Domain.IRepository
{
    public interface IColorRepository : IGenericRepositroy<Color>
    {
        public Task<IEnumerable<Color>> GetAllColorsWithProductInclude();
    }
}
