using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PopFlixBackend._2Application.DTOs;

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

        public Movie()
        {
        }

        public Movie(MovieDTO movieDTO)
        {
            this.Id = movieDTO.Id;
            this.Title = movieDTO.Title;
            this.Year = movieDTO.Year;
            this.Genre = movieDTO.Genre;
        }
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public int Year { get; set; }
    }
}