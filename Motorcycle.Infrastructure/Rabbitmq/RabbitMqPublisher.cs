using System.Text;
using Domain.Interfaces.Services;
using Domain.Models.Dto;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Infrastructure.Rabbitmq
{
    public class RabbitMqPublisher : IPublisher
    {
        private readonly IModel _model;

        public RabbitMqPublisher(IModel model)
        {
            _model = model;
        }

        public void publish(CreateMotorcycleDto body, string exchange, string routingKey)
        {
            var serializedBody = JsonConvert.SerializeObject(body);

            _model.BasicPublish(
                exchange: exchange,
                routingKey: routingKey,
                mandatory: true,
                basicProperties: null,
                body: Encoding.UTF8.GetBytes(serializedBody));
        }
    }
}
