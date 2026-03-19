using StarAtlas.Application.Responses;
using StarAtlas.Application.Dtos.CelestialBodies;
using StarAtlas.Domain.Entities;
using StarAtlas.Infrastructure.Repositories;

namespace StarAtlas.Application.Services
{
    public class CelestialBodyService
    {
        private readonly UnitOfWork _unitOfWork;

        public CelestialBodyService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<CelestialBodyDto>> GetCelestialBodyByIdAsync(int id)
        {
            var body = await _unitOfWork.CelestialBodyRepository.GetByIdWithTypeAsync(id);

            if (body == null)
            {
                return new ApiResponse<CelestialBodyDto>($"Celestial body with ID {id} not found.");
            }

            var dto = new CelestialBodyDto
            {
                Id = body.Id,
                Name = body.Name,
                DistanceLightYears = body.DistanceLightYears,
                Type = body.BodyType?.Name ?? "Unknown"
            };

            return new ApiResponse<CelestialBodyDto>(dto, "Celestial body retrieved successfully.");
        }

        public async Task<ApiResponse<CelestialBodyDto>> CreateCelestialBodyAsync(CreateCelestialBodyDto dto)
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

            var responseDto = new CelestialBodyDto
            {
                Id = celestialBody.Id,
                Name = celestialBody.Name,
                DistanceLightYears = celestialBody.DistanceLightYears,
                Type = "Unknown"
            };

            return new ApiResponse<CelestialBodyDto>(responseDto, "Celestial body created successfully.");
        }

        public async Task<ApiResponse<bool>> UpdateCelestialBodyAsync(int id, UpdateCelestialBodyDto dto)
        {
            if (id != dto.Id)
            {
                return new ApiResponse<bool>("The URL ID does not match the request body ID.");
            }

            var existingBody = await _unitOfWork.CelestialBodyRepository.GetByIdAsync(id);
            if (existingBody == null)
            {
                return new ApiResponse<bool>($"Celestial body with ID {id} not found.");
            }

            existingBody.Name = dto.Name;
            existingBody.Description = dto.Description;
            existingBody.DistanceLightYears = dto.DistanceLightYears;
            existingBody.DiscoveryDate = dto.DiscoveryDate;
            existingBody.BodyTypeId = dto.BodyTypeId;

            _unitOfWork.CelestialBodyRepository.Update(existingBody);
            await _unitOfWork.CompleteAsync();

            return new ApiResponse<bool>(true, "Celestial body updated successfully.");
        }

        public async Task<ApiResponse<bool>> DeleteCelestialBodyAsync(int id)
        {
            var celestialBody = await _unitOfWork.CelestialBodyRepository.GetByIdAsync(id);
            if (celestialBody == null)
            {
                return new ApiResponse<bool>($"Celestial body with ID {id} not found.");
            }

            _unitOfWork.CelestialBodyRepository.Delete(celestialBody);
            await _unitOfWork.CompleteAsync();

            return new ApiResponse<bool>(true, "Celestial body deleted successfully.");
        }
    }
}