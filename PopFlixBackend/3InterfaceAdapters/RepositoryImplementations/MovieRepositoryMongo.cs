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
                gridId = gridId.ToString(),           // Link to GridFS file
                title = title,                        // Human readable title
                contentType = contentType,            // e.g. "video/mp4"
                lengthBytes = length,                 // File size in bytes
                uploadedAt = DateTime.UtcNow,         // Upload timestamp (UTC)
                genre = "Uncategorized",              // Default values for new fields
                year = DateTime.UtcNow.Year           // Default values for new fields
            };

            await _movies.InsertOneAsync(movie);

            // The Id property is automatically set by the InsertOneAsync operation
            return movie;
        }

        // CHANGE 3: Fetch all movie documents, the driver automatically deserializes them to Movie objects
        public async Task<List<Movie>> GetAllAsync();
        public async Task<Movie?> Get(string Id)
        {
            return await _movies.Find<Movie>(m => m.Id == Id)
                            .SortByDescending(m => m.Id)
                            .Limit(1)
                            .FirstOrDefaultAsync();
        }

        public async Task<List<Movie>> GetAll()
        {
            return await _movies.Find(movie => true).ToListAsync();
        }

        //Fetch all movie metadata documents
        public async Task<List<BsonDocument>> GetAllAsync()
        {
            // The Movie entity is used for filtering and the return type
            return await _movies.Find(_ => true).ToListAsync();
        }

        

        public Task Add(Movie movie)
        {
            throw new NotImplementedException();
        }
    }
}