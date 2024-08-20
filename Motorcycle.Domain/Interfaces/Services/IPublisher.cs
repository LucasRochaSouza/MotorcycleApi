using Domain.Models.Dto;

namespace Domain.Interfaces.Services
{
    public interface IPublisher
    {
        void publish(CreateMotorcycleDto body, string exchange, string routingKey);
    }
}
