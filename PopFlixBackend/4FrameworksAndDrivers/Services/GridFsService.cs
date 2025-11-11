using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace PopFlixBackend._4FrameworksAndDrivers.Services
{
    //Service responsible for interacting with MongoDB GridFS
    public class GridFsService
    {
        //GridFS bucket instance
        private readonly GridFSBucket _bucket;

        //Create a GridFS bucket with a configured name and chunck size
        public GridFsService (IMongoDatabase db, IConfiguration cfg) 
        {
            //Get bucket name from configuration or use default "videos"
            var name = cfg["Mongo:Bucket"] ?? "videos";

            //Initialize GridFS bucket with specified options
            _bucket = new GridFSBucket(db, new GridFSBucketOptions
            {
                BucketName = name,
                ChunkSizeBytes = 1024 * 1024 //1 MB chunks
            });
        }

        //Upload a stream to GridFS with optional metadata; returns the file ObjectId
        public Task<ObjectId> UploadAsync(Stream stream, string filename, BsonDocument metadata) =>
            _bucket.UploadFromStreamAsync(filename, stream, new GridFSUploadOptions
            {
                Metadata = metadata
            });
    }
}
