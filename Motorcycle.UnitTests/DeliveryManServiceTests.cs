using Domain.CustomExceptions;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Domain.Services;
using Moq;
public class DeliveryManServiceTests
{
    private readonly Mock<IRentRepository> _rentRepositoryMock;
    private readonly Mock<IDeliveryManRepository> _deliveryManRepositoryMock;
    private readonly Mock<IRentPlanRepository> _rentPlanRepositoryMock;
    private readonly Mock<IMotorcycleRepository> _motorcycleRepositoryMock;
    private readonly DeliveryManService _deliveryManService;

    public DeliveryManServiceTests()
    {
        _rentRepositoryMock = new Mock<IRentRepository>();
        _deliveryManRepositoryMock = new Mock<IDeliveryManRepository>();
        _rentPlanRepositoryMock = new Mock<IRentPlanRepository>();
        _motorcycleRepositoryMock = new Mock<IMotorcycleRepository>();

        _deliveryManService = new DeliveryManService(
            _rentRepositoryMock.Object,
            _deliveryManRepositoryMock.Object,
            _rentPlanRepositoryMock.Object,
            _motorcycleRepositoryMock.Object);
    }

    [Fact]
    public async Task CalculateRentValue_ShouldThrowException_WhenEndDateIsEarlierThanStartDate()
    {
        // Arrange
        var taxPayerId = "123456789";
        var endDate = DateTime.Now;
        var startDate = endDate.AddDays(1);

        _rentRepositoryMock.Setup(r => r.GetCurrentRunningRent(taxPayerId))
            .ReturnsAsync(new Rent { Start = startDate });

        // Act & Assert
        var exception = await Assert.ThrowsAsync<UserException>(() => _deliveryManService.CalculateRentValue(taxPayerId, endDate));
        Assert.Equal("Cannot get rent value if start date is lower then end date", exception.Message);
    }

    [Fact]
    public async Task CalculateRentValue_ShouldCalculateCorrectValue_WhenRentIsOnTime()
    {
        // Arrange
        var taxPayerId = "123456789";
        var startDate = DateTime.Now.AddDays(-10);
        var endDate = DateTime.Now;
        var rentPlan = new RentPlan { DailyPrice = 10 };

        _rentRepositoryMock.Setup(r => r.GetCurrentRunningRent(taxPayerId))
            .ReturnsAsync(new Rent { Start = startDate, EndPrevision = endDate.AddDays(10), RentPlanId = 1 });

        _rentPlanRepositoryMock.Setup(r => r.GetBydId(It.IsAny<long>()))
            .ReturnsAsync(rentPlan);

        // Act
        var result = await _deliveryManService.CalculateRentValue(taxPayerId, endDate);

        // Assert
        Assert.Equal(100, result);
    }

    [Fact]
    public async Task RentMotorcycle_ShouldThrowException_WhenMotorcycleIsNotAvailable()
    {
        // Arrange
        var motorcycleId = 1L;
        var taxPayerId = "123456789";

        _motorcycleRepositoryMock.Setup(m => m.IsMotocycleAvailable(motorcycleId))
            .ReturnsAsync(false);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<UserException>(() => _deliveryManService.RentMotorcyle(motorcycleId, taxPayerId, DateTime.Now));
        Assert.Equal("Motorcycle not available", exception.Message);
    }

    [Fact]
    public async Task RentMotorcycle_ShouldThrowException_WhenLicenseTypeIsInvalid()
    {
        // Arrange
        var motorcycleId = 1L;
        var taxPayerId = "123456789";

        _deliveryManRepositoryMock.Setup(d => d.GetByTaxPayerId(taxPayerId))
            .ReturnsAsync(new DeliveryMan { LicenseType = LicenseTypeEnum.B });

        _motorcycleRepositoryMock.Setup(m => m.IsMotocycleAvailable(motorcycleId))
            .ReturnsAsync(true);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<UserException>(() => _deliveryManService.RentMotorcyle(motorcycleId, taxPayerId, DateTime.Now));
        Assert.Equal("Invalid license type", exception.Message);
    }

    [Fact]
    public async Task UpdateLicense_ShouldCreateDirectoryAndSaveFile()
    {
        // Arrange
        var taxPayerId = "123456789";
        var fileName = "license.jpg";
        var memoryStream = new MemoryStream();
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Licenses", $"{taxPayerId}_{fileName}");

        _deliveryManRepositoryMock.Setup(d => d.UpdateLicenseImage(taxPayerId, It.IsAny<string>()))
            .ReturnsAsync(1);

        // Act
        await _deliveryManService.UpdateLicense(taxPayerId, fileName, memoryStream);

        // Assert
        Assert.True(File.Exists(filePath));
        File.Delete(filePath);
    }

    [Fact]
    public async Task FinishRent_ShouldThrowException_WhenNoRowsAreUpdated()
    {
        // Arrange
        var taxPayerId = "123456789";

        _rentRepositoryMock.Setup(r => r.FinishCurrentRent(taxPayerId))
            .ReturnsAsync(0);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _deliveryManService.FinishRent(taxPayerId));
        Assert.Equal($"Failure finishing rent for motorcycle tax payer id: {taxPayerId}", exception.Message);
    }
}
