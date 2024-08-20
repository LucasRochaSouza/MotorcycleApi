using Domain.Interfaces.Services;
using Domain.Models.Request;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MotorcycleController : ControllerBase
    {
        private readonly IMotorcycleService _motorcycleService;

        public MotorcycleController(IMotorcycleService motorcycleService)
        {
            _motorcycleService = motorcycleService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateMotorcycle([FromBody] CreateMotorcycleRequest request)
        {
            await _motorcycleService.CreateMotorcycle(request);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMotorcycles()
        {
            var motorcycles = await _motorcycleService.GetAllMotorcycles();

            return Ok(motorcycles);
        }

        [HttpGet("{licensePlate}")]
        public async Task<IActionResult> GetMotorcycle(string licensePlate)
        {
            var motorcycle = await _motorcycleService.GetMotorcycle(licensePlate);

            if (motorcycle == null)
            {
                return NotFound("Motorcycle not found.");
            }

            return Ok(motorcycle);
        }

        [HttpPut("{id}/modify")]
        public async Task<IActionResult> ModifyMotorcycle(long id, [FromBody] string licensePlate)
        {
            await _motorcycleService.ModifyMotorcycle(id, licensePlate);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveMotorcycle(long id)
        {
            await _motorcycleService.RemoveMotorcycle(id);

            return Ok();
        }
    }
}
