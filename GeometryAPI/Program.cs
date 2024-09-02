using GeometryLibrary;
using GeometryLibrary.Abstracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using StackExchange.Redis;

namespace GeometryAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            Log.Logger = new LoggerConfiguration()
                        .WriteTo.Console()
                        .CreateLogger();

            builder.Host.UseSerilog();

            // Redis connection
            var multiplexer = ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("RedisConnection"));
            builder.Services.AddSingleton<IConnectionMultiplexer>(multiplexer);
            builder.Services.AddSingleton<IDatabase>(sp => sp.GetRequiredService<IConnectionMultiplexer>().GetDatabase());

            // ILogger ve Redis Cache добавим в ShapeAreaCalculator
            builder.Services.AddScoped(typeof(IShapeAreaCalculator<>), typeof(ShapeAreaCalculator<>));
            //builder.Services.AddSingleton<ILogger>(Log.Logger);

            builder.Services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionReader = new HeaderApiVersionReader("x-api-version"); // Versiyon bilgisini Header'dan al
            });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseDeveloperExceptionPage();
            }
            app.UseSerilogRequestLogging();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
