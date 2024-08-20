using Api.IoC;
using Api.Middlewares;
using Swashbuckle.AspNetCore.SwaggerUI;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        DependencyRegistration.RegisterServices(builder.Services);

        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Logging.AddConsole();

        builder.Services.AddMvc(options =>
        {
            options.Filters.Add<ExceptionMiddleware>();
        });

        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.DocExpansion(DocExpansion.List);
        });

        app.UseRouting();
        app.UseMiddleware<LoggingMiddleware>();

        app.UseEndpoints(x => x.MapControllers());

        app.Run();
    }
}