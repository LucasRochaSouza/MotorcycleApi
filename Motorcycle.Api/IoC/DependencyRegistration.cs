using Domain.CustomExceptions;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Services;
using Infrastructure.Context;
using Infrastructure.Rabbitmq;
using Infrastructure.Rabbitmq.Consumers;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using RabbitMQ.Client;

namespace Api.IoC
{
    public static class DependencyRegistration
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddDbContext();
            services.AddServices();
            services.AddRepositories();
            services.AddFilterExceptions();
            services.AddRabbitMqConnection();
        }

        private static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IMotorcycleService, MotorcycleService>();
            services.AddScoped<IPublisher, RabbitMqPublisher>();
            services.AddScoped<IDeliveryManService, DeliveryManService>();
        }

        private static void AddRepositories(this IServiceCollection services)
        {

            services.AddScoped<IDeliveryManRepository, DeliveryManRepository>();
            services.AddScoped<IMotorcycleRepository, MotorcycleRepository>();
            services.AddScoped<IRentPlanRepository, RentPlanRepository>();
            services.AddScoped<IRentRepository, RentRepository>();
        }

        private static void AddDbContext(this IServiceCollection services)
        {
            services.AddDbContext<DbContext, DatabaseContext>(options =>
                options.UseNpgsql("Server=postgresql;Database=lucas;Username=postgres;Password=postgres"));

            services.AddSingleton(sp =>
            {
                var mongoClient = new MongoClient("mongodb://mongo:27017");
                return mongoClient.GetDatabase("lucas");
            });

            services.BuildServiceProvider().GetService<DatabaseContext>().Database.Migrate();
        }

        private static void AddRabbitMqConnection(this IServiceCollection services)
        {
            services.AddSingleton(sp =>
            {
                var factory = new ConnectionFactory()
                {
                    HostName = "rabbitmq",
                    Port = 5672,
                    UserName = "guest",
                    Password = "guest"
                };

                var connection = factory.CreateConnection();

                var model = connection.CreateModel();

                model.QueueDeclare(queue: "consumer-queue",
                                      durable: false,
                                      exclusive: false,
                                      autoDelete: false,
                arguments: null);

                model.QueueDeclare(queue: "consumer-2024-queue",
                      durable: false,
                      exclusive: false,
                      autoDelete: false,
                      arguments: null);

                model.ExchangeDeclare(exchange: "consumer-exchange", type: ExchangeType.Topic);

                model.QueueBind("consumer-queue", "consumer-exchange", "consumer.*");
                model.QueueBind("consumer-2024-queue", "consumer-exchange", "consumer.2024");

                return model;
            });

            services.AddHostedService<Consumer>();
            services.AddHostedService<Consumer2024>();
        }

        private static void AddFilterExceptions(this IServiceCollection services)
        {
            services.AddSingleton(sp => new Dictionary<Type, Func<Exception, IActionResult>>
            {
                { typeof(UserException), ex => new BadRequestObjectResult(ex.Message) },
                { typeof(KeyNotFoundException), ex => new NotFoundObjectResult(ex.Message) },
                { typeof(Exception), ex => new ObjectResult("Internal Server Error.") { StatusCode = 500 } }
            });
        }
    }
}