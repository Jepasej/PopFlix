using MongoDB.Bson;
using MongoDB.Driver;

namespace PopFlixBackend._3InterfaceAdapters.RepositoryImplementations
{
    /// <summary>
    /// Provides functionality to manage and query movie data stored in a MongoDB database.
    /// </summary>
    /// <remarks>This repository serves as an abstraction layer for interacting with a MongoDB collection
    /// containing movie data. It is designed to encapsulate database operations, making it easier to manage and query
    /// movie records.</remarks>
    public class MovieRepositoryMongo
    {
        //Reference to the MongoDB collection for movies
        private readonly IMongoCollection<BsonDocument> _collection;

        //Constructor that gets reference to the "movies" collection 
        public MovieRepositoryMongo(IMongoDatabase db)
        {
            _collection = db.GetCollection<BsonDocument>("movies");
        }

        // Creates a new movie document in the collection and returns its ID as a string
        public async Task<string> CreateAsync(ObjectId gridId, string title, string contentType, long length)
        {
            //Build metadata document for the movie
            var document = new BsonDocument
            {
                { "gridId", gridId }, // Reference to the GridFS file
                { "title", title },
                { "contentType", contentType }, //type of the video file
                { "lengthBytes", length },
                { "uploadedAt", DateTime.UtcNow } // Timestamp of upload
            };
            //Insert the document into the collection in MongoDB and return the generated ID
            await _collection.InsertOneAsync(document); 
            return document["_id"].ToString();
        }
    }
}
