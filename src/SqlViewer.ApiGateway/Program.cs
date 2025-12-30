using SqlViewer.ApiGateway.Factories;
using SqlViewer.ApiGateway.Services;

namespace SqlViewer.ApiGateway;

public static class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddScoped<ISqlQueryService, SqlQueryService>();
        builder.Services.AddScoped<IMetadataService, MetadataService>();
        builder.Services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        WebApplication app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
