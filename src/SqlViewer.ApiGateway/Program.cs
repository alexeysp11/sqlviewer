using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SqlViewer.ApiGateway.Dtos.FluentValidation;
using SqlViewer.ApiGateway.Mappings;
using SqlViewer.ApiGateway.Middleware;
using SqlViewer.Common.Constants;
using SqlViewer.Security;
using System.Text;

namespace SqlViewer.ApiGateway;

public sealed class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();

        // Add services to the container.
        builder.Services.AddScoped<LoginMapper>();
        builder.Services.AddGrpcClient<SecurityService.SecurityServiceClient>(o =>
        {
            o.Address = new Uri(builder.Configuration[ConfigurationKeys.Services.Grpc.Security]!);
        });

        builder.Services.AddValidatorsFromAssemblyContaining<CreateTableRequestValidator>();
        builder.Services.AddFluentValidationAutoValidation();

        string issuerSigningKey = builder.Configuration.GetValue<string>(ConfigurationKeys.Jwt.Key)
            ?? throw new InvalidOperationException("Unable to get issuer signing key from configurations");
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration[ConfigurationKeys.Jwt.Issuer],

                    ValidateAudience = true,
                    ValidAudience = builder.Configuration[ConfigurationKeys.Jwt.Audience],
                    
                    ValidateLifetime = true,
                    
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(issuerSigningKey))
                };
            });

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
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseMiddleware<GrpcExceptionMiddleware>();

        app.MapControllers();

        app.Run();
    }
}
