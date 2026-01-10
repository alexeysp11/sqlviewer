using Microsoft.EntityFrameworkCore;
using SqlViewer.ApiGateway.DbContexts;
using SqlViewer.ApiGateway.Factories;
using SqlViewer.ApiGateway.Services;
using SqlViewer.ApiGateway.Services.Implementations;

namespace SqlViewer.ApiGateway;

public static class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();

        // Add services to the container.
        builder.Services.AddScoped<ISqlQueryService, SqlQueryService>();
        builder.Services.AddScoped<IMetadataService, MetadataService>();
        builder.Services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();
        builder.Services.AddScoped<IQueryBuilderService, QueryBuilderService>();
        builder.Services.AddScoped<IEncryptionService, EncryptionService>();

        builder.Services.AddDataProtection();
        
        builder.Services.AddDbContext<ApiGatewayDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("MetadataConnection"))
        );

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        WebApplication app = builder.Build();

        // Initialization and seeding the database.
        using (IServiceScope scope = app.Services.CreateScope())
        {
            ApiGatewayDbContext db = scope.ServiceProvider.GetRequiredService<ApiGatewayDbContext>();
            db.Database.Migrate();
        }

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
