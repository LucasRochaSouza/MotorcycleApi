using Domain.Interfaces.Services;
using Domain.Models.Request;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeliveryManController : ControllerBase
    {
        private readonly IDeliveryManService _deliveryManService;

        public DeliveryManController(IDeliveryManService deliveryManService)
        {
            _deliveryManService = deliveryManService;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] CreateDeliveryManRequest request)
        {
            await _deliveryManService.Register(request);
            return Ok();
        }

        [HttpPost("{taxPayerId}/rent")]
        public async Task<IActionResult> RentMotorcycle(long motorcycleId, string taxPayerId, DateTime endPrevision)
        {
            await _deliveryManService.RentMotorcyle(motorcycleId, taxPayerId, endPrevision);
            return Ok();
        }

        [HttpPost("{taxPayerId}/finish")]
        public async Task<IActionResult> FinishRent(string taxPayerId)
        {
            await _deliveryManService.FinishRent(taxPayerId);

            return Ok();
        }

        [HttpGet("{taxPayerId}/calculate")]
        public async Task<IActionResult> CalculateRentValue(string taxPayerId, DateTime endDate)
        {
            var rentValue = await _deliveryManService.CalculateRentValue(taxPayerId, endDate);
            return Ok(rentValue);
        }

        [HttpPost("update-license")]
        public async Task<IActionResult> UpdateLicense(string taxPayerId, IFormFile licenseImage)
        {
            if (licenseImage == null || licenseImage.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            if (licenseImage.ContentType.ToLower() != "image/bmp" && licenseImage.ContentType.ToLower() != "image/png")
            {
                return BadRequest("Invalid mimetype");
            }

            var stream = licenseImage.OpenReadStream();

            await _deliveryManService.UpdateLicense(taxPayerId, licenseImage.FileName, stream);
            return Ok();
        }
    }
}
