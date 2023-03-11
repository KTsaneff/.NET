using Artico.Core.Contracts;
using Artico.Core.Data;
using Artico.Core.Data.Models;
using Artico.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artico.Core.Services
{
    public class ArticleService : IArticleService
    {
        private readonly ApplicationDbContext context;

        public ArticleService(ApplicationDbContext _context)
        {
            this.context = _context;
        }

        public async Task AddArticleAsync(CreateArticleViewModel model)
        {
            var article = new Article()
            {
                Title = model.Title,
                Author = model.Author,
                Body = model.Body,
                IsAthorisationNeeded = model.IsAthorisationNeeded,
                Order = model.Order,
                Tags = model.Tags,
                JobId = model.JobId,
                PositionId = model.PositionId
            };

            await context.Articles.AddAsync(article);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ArticleViewModel>> GetAllAsync()
        {
            var articles = await context.Articles.ToListAsync();

            return articles.Select(a => new ArticleViewModel()
            {
                Id = a.Id,
                Title = a.Title,
                Author = a.Author,
                Body = a.Body,
            });
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
