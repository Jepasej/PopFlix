using PopFlixBackend._1Domain.Entities;

namespace PopFlixBackend._2Application.DTOs
{
    public class MovieDTO
    {
        public string? Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public string Genre { get; set; }

        public MovieDTO()
        {
        }

        public MovieDTO(Movie movie)
        {
            Id = movie.Id;
            Title = movie.title;
            Year = movie.year;
            Genre = movie.genre;
        }
    }
}
