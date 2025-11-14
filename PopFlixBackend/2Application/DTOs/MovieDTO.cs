using PopFlixBackend._1Domain.Entities;

namespace PopFlixBackend._2Application.DTOs
{
    public class MovieDTO
    {
        public string? Id { get; set; }

        public string? gridId { get; set; }
        public string Title { get; set; }
        public string ContentType { get; set; }
        public long LengthBytes { get; set; }
        public DateTime UploadedAt { get; set; }        
        public string Genre { get; set; }
        public int Year { get; set; }


        public MovieDTO()
        {
        }

        // Constructor creates a MovieDTO from a Movie object, used when sending data to the client (Ideally the properties would have been in MovieMetaData)
        public MovieDTO(Movie movie)
        {
            Id = movie.Id;
            gridId = movie.gridId;
            Title = movie.title;
            ContentType = movie.contentType;
            LengthBytes = movie.lengthBytes;
            UploadedAt = movie.uploadedAt;            
            Genre = movie.genre;
            Year = movie.year;
        }
    }
}
