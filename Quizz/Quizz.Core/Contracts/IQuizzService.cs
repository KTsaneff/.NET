using Quizz.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizz.Core.Contracts
{
    public interface IQuizzService
    {
        Task<IEnumerable<QuizzViewModel>> GetQuizzesAsync();

        Task CreateQuizzAsync(QuizzViewModel model);

        Task<QuizzViewModel> GetQuizzAsync(int id);

        Task EditQuizAsync(QuizzViewModel model);

        Task DeleteQuizzAsync(int id);
    }
}
