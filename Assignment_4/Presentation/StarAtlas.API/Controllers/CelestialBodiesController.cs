using Microsoft.AspNetCore.Mvc;
using StarAtlas.Application.Dtos.CelestialBodies;
using StarAtlas.Application.Services;

namespace StarAtlas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CelestialBodiesController : ControllerBase
    {
        private readonly CelestialBodyService _celestialBodyService;

        public CelestialBodiesController(CelestialBodyService celestialBodyService)
        {
            _celestialBodyService = celestialBodyService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCelestialBody(int id)
        {
            var response = await _celestialBodyService.GetCelestialBodyByIdAsync(id);

            if (response == null || !response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> PostCelestialBody(CreateCelestialBodyDto dto)
        {
            var response = await _celestialBodyService.CreateCelestialBodyAsync(dto);

            if (response == null || !response.Success)
            {
                return BadRequest(response);
            }

            return CreatedAtAction(nameof(GetCelestialBody), new { id = response.Data?.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCelestialBody(int id, UpdateCelestialBodyDto dto)
        {
            var response = await _celestialBodyService.UpdateCelestialBodyAsync(id, dto);

            if (response == null || !response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCelestialBody(int id)
        {
            var response = await _celestialBodyService.DeleteCelestialBodyAsync(id);

            if (response == null || !response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}