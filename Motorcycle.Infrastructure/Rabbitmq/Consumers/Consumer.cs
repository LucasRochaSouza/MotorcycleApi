﻿using System.Text;
using Domain.Models.Dto;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Infrastructure.Rabbitmq.Consumers
{
    public class Consumer : BackgroundService
    {
        private readonly IMongoDatabase _mongoClient;
        private readonly IModel _model;

        public Consumer(IMongoDatabase mongoClient, IModel model)
        {
            _mongoClient = mongoClient;
            _model = model;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_model);

            consumer.Received += (model, ea) =>
            {
                if (ea.RoutingKey != "consumer.2024")
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    var objMessage = JsonConvert.DeserializeObject<CreateMotorcycleDto>(message);

                    _mongoClient.GetCollection<CreateMotorcycleDto>("consumer").InsertOne(objMessage);
                }
            };

            _model.BasicConsume(queue: "consumer-queue",
                                 autoAck: true,
                                 consumer: consumer);

            return Task.CompletedTask;
        }
    }
}
