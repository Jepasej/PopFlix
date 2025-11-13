using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PopFlixBackend._1Domain.Entities
{
    /// <summary>
    /// Represents a movie, including its associated metadata such as title, release year, and genre.
    /// </summary>
    /// <remarks>This class serves as a model for storing and managing information about movies.  It can be
    /// used in applications that require movie-related data, such as media libraries, streaming services, or
    /// recommendation systems.</remarks>
    public class Movie
    {
        // Primary Key for the Movies collection
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        // Link to the actual video file stored in MongoDB GridFS (files collection)
        [BsonRepresentation(BsonType.ObjectId)]
        public string GridFileId { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public long LengthBytes { get; set; }
        public DateTime UploadedAt { get; set; }

        // Keeping these from your original entity, assuming they are optional/added later
        public string Genre { get; set; } = string.Empty;
        public int Year { get; set; }
    }
}
