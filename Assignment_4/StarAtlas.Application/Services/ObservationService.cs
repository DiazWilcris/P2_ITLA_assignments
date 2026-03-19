using StarAtlas.Application.Responses;
using StarAtlas.Application.Dtos.Observations;
using StarAtlas.Domain.Entities;
using StarAtlas.Infrastructure.Repositories;

namespace StarAtlas.Application.Services
{
    public class ObservationService
    {
        private readonly UnitOfWork _unitOfWork;

        public ObservationService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<IEnumerable<ObservationDto>>> GetObservationsByStarAsync(int celestialBodyId)
        {
            var observations = await _unitOfWork.ObservationRepository.GetObservationsByStarAsync(celestialBodyId);

            if (!observations.Any())
            {
                return new ApiResponse<IEnumerable<ObservationDto>>($"No observations found for celestial body ID {celestialBodyId}.");
            }

            var dtos = observations.Select(o => new ObservationDto
            {
                Id = o.Id,
                Date = o.ObservationDate,
                Location = o.Location ?? "Unknown",
                Note = o.PersonalNote,
                CelestialBodyName = o.CelestialBody != null ? o.CelestialBody.Name : "Unknown"
            }).ToList();

            return new ApiResponse<IEnumerable<ObservationDto>>(dtos, "Observations retrieved successfully.");
        }

        public async Task<ApiResponse<ObservationDto>> CreateObservationAsync(CreateObservationDto dto)
        {
            var existingBody = await _unitOfWork.CelestialBodyRepository.GetByIdAsync(dto.CelestialBodyId);
            if (existingBody == null)
            {
                return new ApiResponse<ObservationDto>("Celestial Body ID not found.");
            }

            var observation = new Observation
            {
                CelestialBodyId = dto.CelestialBodyId,
                PersonalNote = dto.PersonalNote,
                Location = dto.Location,
                ObservationDate = DateTime.Now
            };

            await _unitOfWork.ObservationRepository.AddAsync(observation);
            await _unitOfWork.CompleteAsync();

            var responseDto = new ObservationDto
            {
                Id = observation.Id,
                Date = observation.ObservationDate,
                Location = observation.Location,
                Note = observation.PersonalNote,
                CelestialBodyName = existingBody.Name
            };

            return new ApiResponse<ObservationDto>(responseDto, "Observation recorded successfully.");
        }

        public async Task<ApiResponse<bool>> UpdateObservationAsync(int id, CreateObservationDto dto)
        {
            var existingObservation = await _unitOfWork.ObservationRepository.GetByIdAsync(id);

            if (existingObservation == null)
            {
                return new ApiResponse<bool>($"Observation with ID {id} not found.");
            }

            existingObservation.PersonalNote = dto.PersonalNote;
            existingObservation.Location = dto.Location;
            existingObservation.ObservationDate = DateTime.Now;

            _unitOfWork.ObservationRepository.Update(existingObservation);
            await _unitOfWork.CompleteAsync();

            return new ApiResponse<bool>(true, "Observation updated successfully.");
        }

        public async Task<ApiResponse<bool>> DeleteObservationAsync(int id)
        {
            var observation = await _unitOfWork.ObservationRepository.GetByIdAsync(id);
            if (observation == null)
            {
                return new ApiResponse<bool>($"Observation with ID {id} not found.");
            }

            _unitOfWork.ObservationRepository.Delete(observation);
            await _unitOfWork.CompleteAsync();

            return new ApiResponse<bool>(true, "Observation deleted successfully.");
        }
    }
}