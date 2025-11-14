using Microsoft.AspNetCore.Builder;
using PopFlixBackend._1Domain.Entities;
using PopFlixBackend._2Application.DTOs;
using PopFlixBackend._2Application.Interfaces;
using PopFlixBackend._4FrameworksAndDrivers.Services;
using MongoDB.Bson;


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

            

            // NEW ENDPOINT: Stream a movie's video content from GridFS
            appMovie.MapGet("/movie/{Id}/stream", async (string Id, IMovieRepository movieRepository, GridFsService grid) =>
            {
                var movie = await movieRepository.Get(Id);

                // Assuming the result from movieRepository.Get(Id) is the Movie entity
                // and it has a public string property called 'GridFsFileId' which stores the ObjectId.
                if (movie == null)
                {
                    return Results.NotFound("Movie metadata not found.");
                }

                // Safely parse the stored GridFS file ID string into an ObjectId
                // NOTE: The 'Movie' entity structure is inferred from Program.cs upload logic.
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