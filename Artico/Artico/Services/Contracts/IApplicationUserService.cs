using Artico.Core.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artico.Core.Contracts
{
    public interface IApplicationUserService
    {
        Task<IEnumerable<Job>> GetJobAsync();

        Task<IEnumerable<Position>> GetPositionAsync();

    }
}
