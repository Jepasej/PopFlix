using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using Microsoft.Extensions.Configuration; // Required for IConfiguration

namespace PopFlixBackend._4FrameworksAndDrivers.Services
{
    //Service responsible for interacting with MongoDB GridFS
    public class GridFsService
    {
        //GridFS bucket instance
        private readonly GridFSBucket _bucket;

        //Create a GridFS bucket with a configured name and chunck size
        public GridFsService(IMongoDatabase db, IConfiguration cfg)
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

        /// <summary>
        /// Retrieves the GridFS download stream, content type, and length for a given file ID.
        /// </summary>
        /// <param name="fileId">The ObjectId of the file stored in GridFS.</param>
        /// <returns>A tuple containing the download stream, content type, and file length, or null if the file is not found.</returns>
        public async Task<(GridFSDownloadStream Stream, string ContentType, long Length)?> GetDownloadStreamAsync(ObjectId fileId)
        {
            // First, try to get the file information using the Find method, as GetInfoAsync is not public.
            var fileInfo = await _bucket.Find(Builders<GridFSFileInfo<ObjectId>>.Filter.Eq("_id", fileId))
                                        .SingleOrDefaultAsync();

            if (fileInfo == null)
            {
                return null;
            }

            // Open the download stream
            var downloadStream = await _bucket.OpenDownloadStreamAsync(fileId);

            // Extract the content type from the metadata, defaulting if missing
            var contentType = fileInfo.Metadata?["contentType"]?.AsString ?? "application/octet-stream";

            return (downloadStream, contentType, fileInfo.Length);
        }
    }
}