namespace Quizz.Infrastructure.Data.Common
{
    public class QuizzRepository : Repository, IQuizzRepository
    {
        public QuizzRepository(ApplicationDbContext context)
            : base(context)
        {
        }
    }
}
