namespace MedUnify.HealthPulseAPI.Services.Concrete
{
    using MedUnify.Domain.HealthPulse;
    using MedUnify.HealthPulseAPI.Repositories.Interface;
    using MedUnify.HealthPulseAPI.Services.Interface;

    public class VisitService : IVisitService
    {
        private readonly IVisitRepository _visitRepository;

        public VisitService(IVisitRepository visitRepository)
        {
            _visitRepository = visitRepository;
        }

        public async Task<List<Visit>> GetVisitsByPatientIdAsync(int patientId)
        {
            return await _visitRepository.GetVisitsByPatientIdAsync(patientId);
        }

        public async Task<Visit> GetVisitByIdAsync(int visitId)
        {
            return await _visitRepository.GetVisitByIdAsync(visitId);
        }

        public async Task AddVisitAsync(Visit visit)
        {
            await _visitRepository.AddVisitAsync(visit);
        }

        public async Task UpdateVisitAsync(Visit visit)
        {
            await _visitRepository.UpdateVisitAsync(visit);
        }

        public async Task DeleteVisitAsync(int visitId)
        {
            await _visitRepository.DeleteVisitAsync(visitId);
        }
    }
}