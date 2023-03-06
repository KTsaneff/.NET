using Artico.Core.Contracts;
using Artico.Core.Data;
using Artico.Core.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artico.Core.Services
{
    public class ApplicationUserService : IApplicationUserService
    {
        private readonly ApplicationDbContext context;

        public ApplicationUserService(ApplicationDbContext _context)
        {
            this.context = _context;
        }

        public async Task<IEnumerable<Job>> GetJobAsync()
        {
            return await context.Jobs.ToListAsync();
        }

        public async Task<IEnumerable<Position>> GetPositionAsync()
        {
            return await context.Positions.ToListAsync();
        }
    }
}
