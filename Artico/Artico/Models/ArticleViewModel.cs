namespace Artico.Models
{
    public class ArticleViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string Author { get; set; } = null!;

        public string Body { get; set; } = null!;
    }
}
