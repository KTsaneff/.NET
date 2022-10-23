using Library.Data.Models;
using Library.Models;

namespace Library.Contracts
{
    public interface IBookService
    {
        Task AddBookAsync(AddBookViewModel model);

        Task<IEnumerable<Category>> GetCategoryAsync();

        Task<IEnumerable<BookViewModel>> GetAllAsync();

        Task AddBookToFavouritesAsync(int bookId, string userId);

        Task<IEnumerable<BookViewModel>> GetFavouritesAsync(string userId);

        Task RemoveBookFromFavouritesAsync(int bookId, string userId);
    }
}
