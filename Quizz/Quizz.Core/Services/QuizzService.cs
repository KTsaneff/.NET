using Microsoft.EntityFrameworkCore;
using Quizz.Core.Contracts;
using Quizz.Core.Models;
using Quizz.Infrastructure.Data.Common;
using Quizz.Infrastructure.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizz.Core.Services
{
    public class QuizzService : IQuizzService
    {
        private readonly IQuizzRepository repo;

        public QuizzService(IQuizzRepository _repo)
        {
            repo = _repo;
        }

        public async Task CreateQuizzAsync(QuizzViewModel model)
        {
            Quiz quizz = new Quiz()
            {
                Name = model.Name
            };

            await repo.AddAsync(quizz);
            await repo.SaveChangesAsync();
        }

        public async Task DeleteQuizzAsync(int id)
        {
            var quizz = await repo.GetByIdAsync<Quiz>(id);

            if (quizz == null)
            {
                throw new ArgumentException("Invalid Quizz ID");
            }

            quizz.IsActive = false;
            await repo.SaveChangesAsync();
        }

        public async Task EditQuizAsync(QuizzViewModel model)
        {
            var quizz = await repo.GetByIdAsync<Quiz>(model.Id);

            if (quizz == null)
            {
                throw new ArgumentException("Invalid Quizz ID");
            }

            quizz.Name = model.Name;
            await repo.SaveChangesAsync();
        }

        public async Task<QuizzViewModel> GetQuizzAsync(int id)
        {
            var quizz = await repo.GetByIdAsync<Quiz>(id);

            if (quizz == null || quizz.IsActive == false)
            {
                throw new ArgumentException("Invalid Quizz ID");
            }

            return new QuizzViewModel()
            {
                Id = quizz.Id,
                Name = quizz.Name
            };
        }

        public async Task<IEnumerable<QuizzViewModel>> GetQuizzesAsync()
        {
            return await repo.AllReadonly<Quiz>()
                .Where(q => q.IsActive)
                .Select(q => new QuizzViewModel()
                {
                    Id = q.Id,
                    Name = q.Name,
                })
                .ToListAsync();
        }
    }
}
