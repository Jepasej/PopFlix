using PopFlixBackend._1Domain.Entities;

namespace PopFlixBackend._2Application.DTOs
{
    public class MovieDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Director { get; set; }
        public int ReleaseYear { get; set; }
        public string Genre { get; set; }

        public MovieDTO()
        {
        }

        public MovieDTO(Movie movie)
        {
            Id = movie.MovieId;
            Title = movie.Title;
            Director = movie.Director;
            ReleaseYear = movie.ReleaseYear;
            Genre = movie.Genre;
        }
    }
}
