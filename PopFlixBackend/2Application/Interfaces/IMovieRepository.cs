using PopFlixBackend._1Domain.Entities;

namespace PopFlixBackend._2Application.Interfaces
{
    /// <summary>
    /// Defines a contract for accessing and managing movie data.
    /// </summary>
    /// <remarks>This interface provides methods for retrieving, adding, updating, and deleting movie records.
    /// Implementations of this interface should handle data persistence and retrieval, allowing consumers  to interact
    /// with movie data without concerning themselves with the underlying storage mechanism.</remarks>
    public interface IMovieRepository
    {
        public Task add(Movie movie);
        public Task<Movie?> Get(int movieId);
        Task<List<Movie>> GetAll();
    }
}
