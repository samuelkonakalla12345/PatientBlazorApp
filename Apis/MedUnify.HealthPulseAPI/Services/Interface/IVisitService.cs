namespace MedUnify.HealthPulseAPI.Services.Interface
{
    using MedUnify.Domain.HealthPulse;

    public interface IVisitService
    {
        Task<List<Visit>> GetVisitsByPatientIdAsync(int patientId);

        Task<Visit> GetVisitByIdAsync(int visitId);

        Task AddVisitAsync(Visit visit);

        Task UpdateVisitAsync(Visit visit);

        Task DeleteVisitAsync(int visitId);
    }
}