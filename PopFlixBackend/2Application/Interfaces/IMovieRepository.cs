using MongoDB.Bson;
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
        // 1. Updated: Returns the full Movie object, which contains the string Id
        Task<Movie> CreateAsync(ObjectId gridId, string title, string contentType, long length);

        // 2. Updated: Returns a List of strongly-typed Movie objects
        Task<List<Movie>> GetAllAsync();
    }
}
