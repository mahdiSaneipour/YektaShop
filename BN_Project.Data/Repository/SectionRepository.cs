using BN_Project.Data.Context;
using BN_Project.Domain.Entities;
using BN_Project.Domain.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BN_Project.Data.Repository
{
    public class SectionRepository : GenericRepository<Section>, ISectionRepository
    {
        private readonly BNContext _context;
        public SectionRepository(BNContext context)
            : base(context)
        {
            _context = context;
        }
    }
}
