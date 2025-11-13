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
        public string? gridId { get; set; } = string.Empty;

        public string title { get; set; } = string.Empty;
        public string contentType { get; set; } = string.Empty;
        public long lengthBytes { get; set; }
        public DateTime uploadedAt { get; set; }

        // Keeping these from your original entity, assuming they are optional/added later
        public string genre { get; set; } = string.Empty;
        public int year { get; set; }


    }
}
