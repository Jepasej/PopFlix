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

            // Endpoint to get all movies
            app.MapGet("/movies", async (IMovieRepository repo) =>
            {
                var movies = await repo.GetAllAsync();
                return Results.Ok(movies);
            });

            // Endpoint to import a movie file
            //app.MapPost("/movies/import", async (HttpRequest req, GridFsService grid, IMovieRepository repo) =>
            //{
            //    // Read multipart form and get the file
            //    var form = await req.ReadFormAsync();
            //    var file = form.Files.GetFile("file");
            //    if (file is null) return Results.BadRequest("file missing");

            //    // Title is optional; fallback to filename
            //    var title = string.IsNullOrWhiteSpace(form["title"]) ? file.FileName : form["title"].ToString();

            //    // Upload to GridFS with minimal metadata
            //    await using var stream = file.OpenReadStream();
            //    var meta = new MongoDB.Bson.BsonDocument { { "contentType", file.ContentType }, { "title", title } };
            //    var gridId = await grid.UploadAsync(stream, file.FileName, meta);

            //    // Store minimal metadata in 'Movies' via repository
            //    var movieId = await repo.CreateAsync(gridId, title, file.ContentType, file.Length);

            //    // Return identifiers
            //    return Results.Ok(new { movieId, gridId = gridId.ToString() });
            //});

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
            //.Produces(StatusCodes.Status200OK);
               
                .DisableAntiforgery();


            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapWeatherEndpoints();

            //app.MapMovieEndpoints();

            app.Run();
        }
    }
}
