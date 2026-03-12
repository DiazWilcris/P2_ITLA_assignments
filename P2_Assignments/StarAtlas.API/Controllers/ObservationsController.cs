using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarAtlas.API.Models.Dtos;
using StarAtlas.Domain.Entities;
using StarAtlas.Infrastructure.Repositories;

namespace StarAtlas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ObservationsController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        public ObservationsController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("star/{celestialBodyId}")]
        public async Task<ActionResult<IEnumerable<ObservationDto>>> GetObservationsByBody(int celestialBodyId)
        {
            var observations = await _unitOfWork.ObservationRepository.GetObservationsByStarAsync(celestialBodyId);

            if (!observations.Any()) return NotFound("No observations found for this celestial body.");

            var dtos = observations.Select(o => new ObservationDto
            {
                Id = o.Id,
                Date = o.ObservationDate,
                Location = o.Location ?? "Unknown",
                Note = o.PersonalNote,
                CelestialBodyName = o.CelestialBody != null ? o.CelestialBody.Name : "Unknown"
            }).ToList();

            return Ok(dtos);
        }

        [HttpPost]
        public async Task<ActionResult<ObservationDto>> CreateObservation(CreateObservationDto dto)
        {
            var existingBody = await _unitOfWork.CelestialBodyRepository.GetByIdAsync(dto.CelestialBodyId);
            if (existingBody == null) return BadRequest("Celestial Body ID not found.");

            var observation = new Observation
            {
                CelestialBodyId = dto.CelestialBodyId,
                PersonalNote = dto.PersonalNote,
                Location = dto.Location,
                ObservationDate = DateTime.Now
            };

            await _unitOfWork.ObservationRepository.AddAsync(observation);
            await _unitOfWork.CompleteAsync();

            return CreatedAtAction(nameof(GetObservationsByBody),
                new { celestialBodyId = observation.CelestialBodyId },
                new { Message = "Observation recorded successfully!", Id = observation.Id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateObservation(int id, [FromBody] CreateObservationDto dto)
        {
            var existingObservation = await _unitOfWork.ObservationRepository.GetByIdAsync(id);

            if (existingObservation == null) return NotFound("Observation not found.");

            existingObservation.PersonalNote = dto.PersonalNote;
            existingObservation.Location = dto.Location;
            existingObservation.ObservationDate = DateTime.Now;

            _unitOfWork.ObservationRepository.Update(existingObservation);

            try
            {
                await _unitOfWork.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException) { throw; }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteObservation(int id)
        {
            var observation = await _unitOfWork.ObservationRepository.GetByIdAsync(id);
            if (observation == null) return NotFound("Observation not found.");

            _unitOfWork.ObservationRepository.Delete(observation);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }
    }
}