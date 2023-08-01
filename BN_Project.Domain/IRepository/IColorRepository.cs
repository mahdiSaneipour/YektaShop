using BN_Project.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BN_Project.Domain.IRepository
{
    public interface IColorRepository
    {
        public IQueryable<Color> GetAllColors(Expression<Func<Color, bool>> where = null);

        public Color GetColorByColorId(int colorId);

        public IQueryable<Color> GetColorsByProductId(int productId);

        public void AddColor(Color color);

        public void UpdateColor(Color color);

        public void SaveChanges();
    }
}
