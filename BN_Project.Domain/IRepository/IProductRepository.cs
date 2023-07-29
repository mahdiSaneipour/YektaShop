using BN_Project.Domain.Entities;
using BN_Project.Domain.ViewModel.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BN_Project.Domain.IRepository
{
    public interface IProductRepository
    {
        public Task<IQueryable<Product>> GetProducts();
    }
}
