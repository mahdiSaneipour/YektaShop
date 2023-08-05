﻿using BN_Project.Data.Context;
using BN_Project.Domain.Entities;
using BN_Project.Domain.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BN_Project.Data.Repository
{
    public class ColorRepository : GenericRepository<Color>, IColorRepository
    {
        private readonly BNContext _context;

        public ColorRepository(BNContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Color>> GetAllColorsWithProductInclude()
        {
            return await _context.Colors.Include(c => c.Product).ToListAsync();
        }
    }
}
