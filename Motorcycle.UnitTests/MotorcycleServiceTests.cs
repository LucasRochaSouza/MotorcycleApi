using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Models.Dto;
using Domain.Models.Request;
using Domain.Services;
using Moq;

public class MotorcycleServiceTests
{
    private readonly Mock<IMotorcycleRepository> _motorcycleRepositoryMock;
    private readonly Mock<IRentRepository> _rentRepositoryMock;
    private readonly Mock<IPublisher> _publisherMock;
    private readonly MotorcycleService _motorcycleService;

    public MotorcycleServiceTests()
    {
        _motorcycleRepositoryMock = new Mock<IMotorcycleRepository>();
        _rentRepositoryMock = new Mock<IRentRepository>();
        _publisherMock = new Mock<IPublisher>();
        _motorcycleService = new MotorcycleService(
            _motorcycleRepositoryMock.Object,
            _rentRepositoryMock.Object,
            _publisherMock.Object);
    }

    [Fact]
    public async Task CreateMotorcycle_ShouldCallRepositoryAndPublisher()
    {
        // Arrange
        var request = new CreateMotorcycleRequest
        {
            LicensePlate = "ABC123",
            Model = "ModelX",
            Year = 2024
        };

        // Act
        await _motorcycleService.CreateMotorcycle(request);

        // Assert
        _motorcycleRepositoryMock.Verify(m => m.Create(It.IsAny<Motorcycle>()), Times.Once);
        _publisherMock.Verify(p => p.publish(It.IsAny<CreateMotorcycleDto>(), "consumer-exchange", "consumer.2024"), Times.Once);
    }

    [Fact]
    public async Task GetAllMotorcycles_ShouldReturnMotorcycles()
    {
        // Arrange
        var motorcycles = new List<Motorcycle>
        {
            new Motorcycle { Id = 1, LicensePlate = "ABC123", Model = "ModelX", Year = 2024 },
            new Motorcycle { Id = 2, LicensePlate = "DEF456", Model = "ModelY", Year = 2023 }
        };
        _motorcycleRepositoryMock.Setup(m => m.GetMotorcycles()).ReturnsAsync(motorcycles);

        // Act
        var result = await _motorcycleService.GetAllMotorcycles();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("ABC123", result[0].LicensePlate);
        Assert.Equal("DEF456", result[1].LicensePlate);
    }

    [Fact]
    public async Task GetMotorcycle_ShouldReturnCorrectMotorcycle()
    {
        // Arrange
        var motorcycle = new Motorcycle { Id = 1, LicensePlate = "ABC123", Model = "ModelX", Year = 2024 };
        _motorcycleRepositoryMock.Setup(m => m.GetMotorcycle("ABC123")).ReturnsAsync(motorcycle);

        // Act
        var result = await _motorcycleService.GetMotorcycle("ABC123");

        // Assert
        Assert.Equal("ModelX", result.Model);
        Assert.Equal((uint)2024, result.Year);
    }

    [Fact]
    public async Task ModifyMotorcycle_ShouldThrowException_WhenRentsExist()
    {
        // Arrange
        var id = 1L;
        var licensePlate = "NEW123";
        _rentRepositoryMock.Setup(r => r.GetAll()).ReturnsAsync(new List<Rent> { new Rent() });

        // Act/Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _motorcycleService.ModifyMotorcycle(id, licensePlate));
        Assert.Equal($"Error updating motorcycle. No motorcycle found for given id: {id}", exception.Message);
    }

    [Fact]
    public async Task ModifyMotorcycle_ShouldUpdateMotorcycle_WhenNoRentsExist()
    {
        // Arrange
        var id = 1L;
        var licensePlate = "NEW123";
        _rentRepositoryMock.Setup(r => r.GetAll()).ReturnsAsync(new List<Rent>());
        _motorcycleRepositoryMock.Setup(m => m.UpdateMotorcycle(id, licensePlate)).ReturnsAsync(1);

        // Act
        await _motorcycleService.ModifyMotorcycle(id, licensePlate);

        // Assert
        _motorcycleRepositoryMock.Verify(m => m.UpdateMotorcycle(id, licensePlate), Times.Once);
    }

    [Fact]
    public async Task RemoveMotorcycle_ShouldThrowException_WhenRentsExist()
    {
        // Arrange
        var id = 1L;
        _rentRepositoryMock.Setup(r => r.GetAll()).ReturnsAsync(new List<Rent> { new Rent() });

        // Act & Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _motorcycleService.RemoveMotorcycle(id));
        Assert.Equal($"Error removing motorcycle. No motorcycle found for given id: {id}", exception.Message);
    }

    [Fact]
    public async Task RemoveMotorcycle_ShouldDeleteMotorcycle_WhenNoRentsExist()
    {
        // Arrange
        var id = 1L;
        _rentRepositoryMock.Setup(r => r.GetAll()).ReturnsAsync(new List<Rent>());
        _motorcycleRepositoryMock.Setup(m => m.DeleteMotorcycle(id)).ReturnsAsync(1);

        // Act
        await _motorcycleService.RemoveMotorcycle(id);

        // Assert
        _motorcycleRepositoryMock.Verify(m => m.DeleteMotorcycle(id), Times.Once);
    }
}
