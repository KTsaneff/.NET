using Artico.Core.Data.Models;
using Artico.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artico.Core.Contracts
{
    public interface IArticleService
    {
        Task<IEnumerable<ArticleViewModel>> GetAllAsync();

        Task<IEnumerable<Job>> GetJobAsync();

        Task<IEnumerable<Position>> GetPositionAsync();

        Task AddArticleAsync(CreateArticleViewModel model);

    }
}
