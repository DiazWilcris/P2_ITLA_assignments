using Microsoft.AspNetCore.Mvc;
using StarAtlas.Application.Dtos.Observations;
using StarAtlas.Application.Services;

namespace StarAtlas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ObservationsController : ControllerBase
    {
        private readonly ObservationService _observationService;

        public ObservationsController(ObservationService observationService)
        {
            _observationService = observationService;
        }

        [HttpGet("star/{celestialBodyId}")]
        public async Task<IActionResult> GetObservationsByBody(int celestialBodyId)
        {
            var response = await _observationService.GetObservationsByStarAsync(celestialBodyId);

            if (response == null || !response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateObservation(CreateObservationDto dto)
        {
            var response = await _observationService.CreateObservationAsync(dto);

            if (response == null || !response.Success)
            {
                return BadRequest(response);
            }

            // Route name matches the GET method, passing the required parameter
            return CreatedAtAction(nameof(GetObservationsByBody), new { celestialBodyId = dto.CelestialBodyId }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateObservation(int id, [FromBody] CreateObservationDto dto)
        {
            var response = await _observationService.UpdateObservationAsync(id, dto);

            if (response == null || !response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteObservation(int id)
        {
            var response = await _observationService.DeleteObservationAsync(id);

            if (response == null || !response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}