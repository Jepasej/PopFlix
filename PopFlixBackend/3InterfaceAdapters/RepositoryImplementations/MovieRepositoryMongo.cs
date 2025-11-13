using MongoDB.Bson;
using MongoDB.Driver;
using PopFlixBackend._1Domain.Entities;
using PopFlixBackend._2Application.Interfaces;
using System.Text.RegularExpressions;

namespace PopFlixBackend._3InterfaceAdapters.RepositoryImplementations
{
    /// <summary>
    /// Repository for storing minimal movie metadata in MongoDB.
    /// </summary>
    public class MovieRepositoryMongo : IMovieRepository
    {
        // CHANGE 1: Reference to the "movies" collection, now strongly typed to Movie
        private readonly IMongoCollection<Movie> _movies;

        // CHANGE 2: Update constructor to resolve collection using the Movie type
        public MovieRepositoryMongo(IMongoDatabase db)
        {
            _movies = db.GetCollection<Movie>("movies");
        }

        /// <summary>
        /// Inserts movie metadata into the database and returns the created Movie object.
        /// </summary>
        public async Task<Movie> CreateAsync(ObjectId gridId, string title, string contentType, long length)
        {
            var movie = new Movie
            {
                // Note: Id is left null, the MongoDB driver will assign the BsonObjectId and convert it to string
                GridFileId = gridId.ToString(),       // Link to GridFS file
                Title = title,                        // Human readable title
                ContentType = contentType,            // e.g. "video/mp4"
                LengthBytes = length,                 // File size in bytes
                UploadedAt = DateTime.UtcNow,         // Upload timestamp (UTC)
                Genre = "Uncategorized",              // Default values for new fields
                Year = DateTime.UtcNow.Year           // Default values for new fields
            };

            await _movies.InsertOneAsync(movie);

            // The Id property is automatically set by the InsertOneAsync operation
            return movie;
        }

        // CHANGE 3: Fetch all movie documents, the driver automatically deserializes them to Movie objects
        public async Task<List<Movie>> GetAllAsync()
        {
            // The Movie entity is used for filtering and the return type
            return await _movies.Find(_ => true).ToListAsync();
        }
    }
}