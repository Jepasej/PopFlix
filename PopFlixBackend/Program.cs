using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using PopFlixBackend._1Domain.Entities;
using PopFlixBackend._2Application.Interfaces;
using PopFlixBackend._3InterfaceAdapters.RepositoryImplementations;
using PopFlixBackend._4FrameworksAndDrivers.Endpoints;
using PopFlixBackend._4FrameworksAndDrivers.Services;
using Microsoft.AspNetCore.Antiforgery;
using PopFlixBackend._2Application.DTOs;


namespace PopFlixBackend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("MongoDb")
                                    ?? "mongodb://localhost:27017";
            var mongoClient = new MongoClient(connectionString);
            var database = mongoClient.GetDatabase("PopFlixDb");

            builder.Services.AddSingleton<IMongoDatabase>(database);
            builder.Services.AddSingleton<GridFsService>();
            builder.Services.AddSingleton<IMovieRepository, MovieRepositoryMongo>();

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();

                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/openapi/v1.json", app.Environment.ApplicationName);
                });
            }
            app.UseAuthorization();

            // Endpoint to get all movies
            app.MapGet("/movies", async (IMovieRepository repo) =>
            {
                var movies = await repo.GetAllAsync();
                return Results.Ok(movies);
            });
            
            app.MapPost("/movies/import", async ([FromForm] IFormFile file, [FromForm] string? title, GridFsService grid, IMovieRepository repo) =>
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


            app.UseHttpsRedirection();

            app.MapMovieEndpoints();

            app.Run();
        }
    }
}