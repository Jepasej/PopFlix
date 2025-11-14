using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using PopFlixBackend._1Domain.Entities;
using PopFlixBackend._2Application.DTOs;
using PopFlixBackend._2Application.Interfaces;
using PopFlixBackend._4FrameworksAndDrivers.Services;


namespace PopFlixBackend._4FrameworksAndDrivers.Endpoints

{
    /// <summary>
    /// Provides extension methods for mapping movie-related endpoints to an <see cref="IEndpointRouteBuilder"/>.
    /// </summary>
    /// <remarks>This static class contains methods to map endpoints for retrieving movie details and
    /// streaming movie video content. The endpoints are designed to work with an <see cref="IMovieRepository"/> for
    /// accessing movie data and a <see cref="GridFsService"/> for streaming video content.</remarks>
    public static class MovieEndpoints
    {
        public static IEndpointRouteBuilder MapMovieEndpoints(this IEndpointRouteBuilder appMovie)
        {

            // Endpoint to import a movie file
            appMovie.MapPost("/movies/import", async ([FromForm] IFormFile file, [FromForm] string? title, GridFsService grid, IMovieRepository repo) =>
            {
                if (file is null) return Results.BadRequest("file missing");

                var resolvedTitle = string.IsNullOrWhiteSpace(title) ? file.FileName : title;

                await using var stream = file.OpenReadStream();
                var meta = new BsonDocument {
                { "contentType", file.ContentType },
                { "title", resolvedTitle }
            };

                var gridId = await grid.UploadAsync(stream, file.FileName, meta);
                var movieId = await repo.CreateAsync(gridId, resolvedTitle, file.ContentType, file.Length);

                return Results.Ok(new { movieId, gridId = gridId.ToString() });
            })
            .DisableAntiforgery();

            // Endpoint to get all movies
            appMovie.MapGet("/movies", async (IMovieRepository repo) =>
            {
                var movies = await repo.GetAllAsync();
                return Results.Ok(movies);
            });

            //Get a movie by ID
            appMovie.MapGet("/movies/{Id}", async (string Id, IMovieRepository movieRepository) =>
            {
                var movie = await movieRepository.Get(Id);
                if (movie == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(new MovieDTO(movie));
            })
            .WithName("GetMovieById");

            

            //Stream a movie's video content from GridFS
            appMovie.MapGet("/movies/{Id}/stream", async (string Id, IMovieRepository movieRepository, GridFsService grid) =>
            {
                var movie = await movieRepository.Get(Id);

                // Assuming the result from movieRepository.Get(Id) is the Movie entity
                // and it has a public string property called 'GridFsFileId' which stores the ObjectId.
                if (movie == null)
                {
                    return Results.NotFound("Movie metadata not found.");
                }

                // Safely parse the stored GridFS file ID string into an ObjectId                
                if (!ObjectId.TryParse(movie.gridId, out var fileId))
                {
                    return Results.BadRequest("Invalid GridFS file ID associated with the movie.");
                }

                var downloadResult = await grid.GetDownloadStreamAsync(fileId);

                if (downloadResult == null)
                {
                    return Results.NotFound("Video file not found in GridFS.");
                }

                // Use Results.File to stream the content.
                // enableRangeProcessing: true is CRITICAL for video players to seek (HTTP 206 Partial Content).
                return Results.File(
                    downloadResult.Value.Stream,
                    contentType: downloadResult.Value.ContentType,
                    fileDownloadName: movie.title, // Optional: suggests a file name for download clients
                    enableRangeProcessing: true
                );
            })
            .WithName("StreamMovieVideo");           


            return appMovie;
        }
    }

}