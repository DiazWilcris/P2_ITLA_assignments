using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarAtlas.API.Models.Dtos;
using StarAtlas.Domain.Entities;
using StarAtlas.Infrastructure.Repositories;

namespace StarAtlas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BodyTypesController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        public BodyTypesController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BodyTypeDto>>> GetBodyTypes()
        {
            var types = await _unitOfWork.BodyTypeRepository.GetAllAsync();

            var dtos = types.Select(t => new BodyTypeDto
            {
                Id = t.Id,
                Name = t.Name
            }).ToList();

            return Ok(dtos);
        }

        [HttpPost]
        public async Task<ActionResult<BodyType>> PostBodyType(BodyType bodyType)
        {
            await _unitOfWork.BodyTypeRepository.AddAsync(bodyType);
            await _unitOfWork.CompleteAsync();
            return CreatedAtAction("GetBodyTypes", new { id = bodyType.Id }, bodyType);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBodyType(int id, BodyType bodyType)
        {
            if (id != bodyType.Id) return BadRequest("The ID does not match the ID in the body.");

            _unitOfWork.BodyTypeRepository.Update(bodyType);

            try
            {
                await _unitOfWork.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                var exists = await _unitOfWork.BodyTypeRepository.GetByIdAsync(id) != null;
                if (!exists) return NotFound();
                else throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBodyType(int id)
        {
            var bodyType = await _unitOfWork.BodyTypeRepository.GetByIdAsync(id);
            if (bodyType == null) return NotFound();

            var allBodies = await _unitOfWork.CelestialBodyRepository.GetAllAsync();
            var isUsed = allBodies.Any(c => c.BodyTypeId == id);

            if (isUsed) return BadRequest("Cannot delete this type because it is assigned to existing Celestial Bodies. Delete the stars first.");

            _unitOfWork.BodyTypeRepository.Delete(bodyType);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }
    }
}