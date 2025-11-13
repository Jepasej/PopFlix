
using Microsoft.AspNetCore.Builder;
using PopFlixBackend._2Application.Interfaces;


namespace PopFlixBackend._4FrameworksAndDrivers.Endpoints

{
    public class MovieEndpoints
    {
        public static IEndpointRouteBuilder MapMovieEndpoints(this IEndpointRouteBuilder appMovie)
        {
            appMovie.MapGet("/movie/{movieId}", async (int movieId, IMovieRepository movieRepository) =>
            {
                var movie = await movieRepository.Get(movieId);
                if (movie == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(movie);
            })
                
            
            return appMovie;
        }
        
}
