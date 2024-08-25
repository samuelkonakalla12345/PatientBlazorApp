namespace MedUnify.HealthPulseAPI.Repositories.Interface
{
    using MedUnify.Domain.HealthPulse;

    public interface IVisitRepository
    {
        Task<List<Visit>> GetVisitsByPatientIdAsync(int patientId);

        Task<Visit> GetVisitByIdAsync(int visitId);

        Task AddVisitAsync(Visit visit);

        Task UpdateVisitAsync(Visit visit);

        Task DeleteVisitAsync(int visitId);
    }
}