using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarAtlas.API.Models.Dtos;
using StarAtlas.Domain.Entities;
using StarAtlas.Infrastructure.Repositories;

namespace StarAtlas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CelestialBodiesController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        public CelestialBodiesController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CelestialBodyDto>> GetCelestialBody(int id)
        {
            var body = await _unitOfWork.CelestialBodyRepository.GetByIdWithTypeAsync(id);

            if (body == null) return NotFound();

            var dto = new CelestialBodyDto
            {
                Id = body.Id,
                Name = body.Name,
                DistanceLightYears = body.DistanceLightYears,
                Type = body.BodyType?.Name ?? "Unknown"
            };

            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<CelestialBody>> PostCelestialBody(CreateCelestialBodyDto dto)
        {
            var celestialBody = new CelestialBody
            {
                Name = dto.Name,
                Description = dto.Description,
                DistanceLightYears = dto.DistanceLightYears,
                DiscoveryDate = dto.DiscoveryDate,
                BodyTypeId = dto.BodyTypeId
            };

            await _unitOfWork.CelestialBodyRepository.AddAsync(celestialBody);
            await _unitOfWork.CompleteAsync();

            return CreatedAtAction("GetCelestialBody", new { id = celestialBody.Id }, celestialBody);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCelestialBody(int id, UpdateCelestialBodyDto dto)
        {
            if (id != dto.Id) return BadRequest("El ID de la URL no coincide con el ID del cuerpo del mensaje.");

            var existingBody = await _unitOfWork.CelestialBodyRepository.GetByIdAsync(id);
            if (existingBody == null) return NotFound($"No se encontró el astro con ID {id}");

            existingBody.Name = dto.Name;
            existingBody.Description = dto.Description;
            existingBody.DistanceLightYears = dto.DistanceLightYears;
            existingBody.DiscoveryDate = dto.DiscoveryDate;
            existingBody.BodyTypeId = dto.BodyTypeId;

            _unitOfWork.CelestialBodyRepository.Update(existingBody);

            try
            {
                await _unitOfWork.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException) { throw; }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCelestialBody(int id)
        {
            var celestialBody = await _unitOfWork.CelestialBodyRepository.GetByIdAsync(id);
            if (celestialBody == null) return NotFound($"Celestial Body with ID {id} not found.");

            _unitOfWork.CelestialBodyRepository.Delete(celestialBody);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }
    }
}
