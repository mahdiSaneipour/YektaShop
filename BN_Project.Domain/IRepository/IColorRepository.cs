using BN_Project.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BN_Project.Domain.IRepository
{
    public interface IColorRepository
    {
        public Task<Color> GetColorByColorId();

        public Task<IQueryable<Color>> GetColorsByProductId(int productId);

        public Task AddColor(Color color);

        public void UpdateColor(Color color);

        public Task SaveChanges();
    }
}
