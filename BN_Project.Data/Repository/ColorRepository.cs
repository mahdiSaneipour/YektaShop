﻿using BN_Project.Data.Context;
using BN_Project.Domain.Entities;
using BN_Project.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BN_Project.Data.Repository
{
    public class ColorRepository : IColorRepository
    {
        private readonly BNContext _context;

        public ColorRepository(BNContext context)
        {
            _context = context;
        }

        public Task AddColor(Color color)
        {
            throw new NotImplementedException();
        }

        public Task<Color> GetColorByColorId()
        {
            throw new NotImplementedException();
        }

        public Task<IQueryable<Color>> GetColorsByProductId(int productId)
        {
            throw new NotImplementedException();
        }

        public Task SaveChanges()
        {
            throw new NotImplementedException();
        }

        public Task UpdateColor(Color color)
        {
            throw new NotImplementedException();
        }
    }
}
