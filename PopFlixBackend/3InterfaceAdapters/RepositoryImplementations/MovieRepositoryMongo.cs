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
        // Reference to the "movies" collection (holds metadata, not the GridFS chunks/files)
        private readonly IMongoCollection<BsonDocument> _movies;
        private readonly IMongoCollection<Movie> _movieEntity;

        // Resolve the "movies" collection from the provided database
        public MovieRepositoryMongo(IMongoDatabase db)
        {
            _movies = db.GetCollection<BsonDocument>("movies");
            _movieEntity = db.GetCollection<Movie>("movieEntity");
        }

        public async Task Add(Movie movie)
        {
            await _movieEntity.InsertOneAsync(movie);
        }

        // Insert  metadata and return the created document id as string
        public async Task<string> CreateAsync(ObjectId gridId, string title, string contentType, long length)
        {
            var doc = new BsonDocument
            {
                { "gridId", gridId },             // link to GridFS file (_id from videos.files)
                { "title", title },               // human readable title
                { "contentType", contentType },   // e.g. "video/mp4"
                { "lengthBytes", length },        // file size in bytes
                { "uploadedAt", DateTime.UtcNow } // upload timestamp (UTC)
            };

            await _movies.InsertOneAsync(doc);
            return doc["_id"].ToString();
        }

        public async Task<Movie?> Get(string Id)
        {
            return await _movieEntity.Find<Movie>(m => m.Id == Id)
                            .SortByDescending(m => m.Id)
                            .Limit(1)
                            .FirstOrDefaultAsync();
        }

        public async Task<List<Movie>> GetAll()
        {
            return await _movieEntity.Find(movie => true).ToListAsync();
        }

        //Fetch all movie metadata documents
        public async Task<List<BsonDocument>> GetAllAsync()
        {
            return await _movies.Find(FilterDefinition<BsonDocument>.Empty).ToListAsync();
            // alternative: return await _movies.Find(_ => true).ToListAsync();
        }


    }
}