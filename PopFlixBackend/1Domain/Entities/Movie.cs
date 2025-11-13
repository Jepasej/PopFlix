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
        public int MovieId { get; set; }
        public string Title { get; set; }
        public string Director { get; set; }
        public int ReleaseYear { get; set; }
        public string Genre { get; set; }   

        public Movie()
        {
        }

        public Movie(int movieId, string title, string director, int releaseYear, string genre)
        {
            MovieId = movieId;
            Title = title;
            Director = director;
            ReleaseYear = releaseYear;
            Genre = genre;
        }
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public int Year { get; set; }
    }
}
