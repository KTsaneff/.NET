using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Watchlist.Contracts;
using Watchlist.Data;
using Watchlist.Data.Models;
using Watchlist.Models;

namespace Watchlist.Services
{
    public class MovieService : IMovieService
    {
        private readonly WatchlistDbContext context;

        public MovieService(WatchlistDbContext _context)
        {
            this.context = _context;
        }

        public async Task AddMovieAsync(AddMovieViewModel model)
        {
            var entity = new Movie()
            {
                Director = model.Director,
                GenreId = model.GenreId,
                Title = model.Title,
                ImageUrl = model.ImageUrl,
                Rating = model.Rating
            };

            await context.Movies.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task AddMovieToCollectionAsync(int movieId, string userId)
        {
            var user = await context.Users
                .Where(u => u.Id == userId)
                .Include(u => u.UsersMovies)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new ArgumentException("Invalid user ID");
            }

            var movie = await context.Movies.FirstOrDefaultAsync(u => u.Id == movieId);

            if (movie == null)
            {
                throw new ArgumentException("Invalid movie ID");
            }

            if (!user.UsersMovies.Any(m => m.MovieId == movieId))
            {
                user.UsersMovies.Add(new UserMovie()
                {
                    MovieId = movieId,
                    UserId = userId,
                    Movie = movie,
                    User = user
                });
            }            

            await context.SaveChangesAsync();
        }

        public async Task EditAsync(EditMovieViewModel model)
        {
            var entity = await context.Movies.FindAsync(model.Id);

            entity.Rating = model.Rating;
            entity.ImageUrl = model.ImageUrl;
            entity.Director = model.Director;
            entity.Title = model.Title;
            entity.GenreId = model.GenreId;

            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<MovieViewModel>> GetAllAsync()
        {
            var entities = await context.Movies
                .Include(m => m.Genre).ToListAsync();

            return entities.Select(m => new MovieViewModel
            {
                Id = m.Id,
                Title = m.Title,
                Director = m.Director,
                Genre = m.Genre?.Name,
                ImageUrl = m.ImageUrl,
                Rating = m.Rating
            });
        }

        public async Task<EditMovieViewModel> GetForEditAsync(int id)
        {
            var movie = await context.Movies.FindAsync(id);

            var model =  new EditMovieViewModel()
            {
                Id = id,
                Director = movie.Director,
                GenreId = movie.GenreId ?? -1,
                ImageUrl = movie.ImageUrl,
                Rating= movie.Rating,
                Title = movie.Title
            };

            model.Genres = await GetGenresAsync();

            return model;
        }

        public async Task<IEnumerable<Genre>> GetGenresAsync()
        {
            return await context.Genres.ToListAsync();
        }

        public async Task<IEnumerable<MovieViewModel>> GetWatchedAsync(string userId)
        {
            var user = await context.Users
                .Where(u => u.Id == userId)
                .Include(u => u.UsersMovies)
                .ThenInclude(um => um.Movie)
                .ThenInclude(m => m.Genre)
                .FirstOrDefaultAsync();


            if (user == null)
            {
                throw new ArgumentException("Invalid user ID");
            }

            return user.UsersMovies.Select(m => new MovieViewModel()
            {
                Director = m.Movie.Director,
                Genre = m.Movie.Genre?.Name,
                Id = m.MovieId,
                ImageUrl = m.Movie.ImageUrl,
                Rating = m.Movie.Rating,
                Title = m.Movie.Title
            });
        }

        public async Task RemoveMovieFromCollectionAsync(int movieId, string userId)
        {
            var user = await context.Users
                .Where(u => u.Id == userId)
                .Include(u => u.UsersMovies)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new ArgumentException("Invalid user ID");
            }

            var movie = user.UsersMovies.FirstOrDefault(m => m.MovieId == movieId);

            if (movie != null)
            {
                user.UsersMovies.Remove(movie);
                await context.SaveChangesAsync();
            }
        }
    }
}
