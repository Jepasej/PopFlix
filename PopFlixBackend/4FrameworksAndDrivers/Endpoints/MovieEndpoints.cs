using Microsoft.AspNetCore.Builder;
using PopFlixBackend._1Domain.Entities;
using PopFlixBackend._2Application.DTOs;
using PopFlixBackend._2Application.Interfaces;


namespace PopFlixBackend._4FrameworksAndDrivers.Endpoints

{
    public static class MovieEndpoints
    {
        public static IEndpointRouteBuilder MapMovieEndpoints(this IEndpointRouteBuilder appMovie)
        {
            appMovie.MapGet("/movie/{Id}", async (string Id, IMovieRepository movieRepository) =>
            {
                var movie = await movieRepository.Get(Id);
                if (movie == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(new MovieDTO(movie));
            })
            .WithName("GetMovieById");

            appMovie.MapPost("/movie", async (MovieDTO movieDto, IMovieRepository movieRepository) =>
            {
                await movieRepository.Add(new Movie(movieDto));

                return Results.Created();
            })
            .WithName("Post Movie");

            return appMovie;
        }
    }

}