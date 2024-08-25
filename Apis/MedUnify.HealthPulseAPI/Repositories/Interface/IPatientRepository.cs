namespace MedUnify.HealthPulseAPI.Repositories.Interface
{
    using MedUnify.Domain.HealthPulse;

    public interface IPatientRepository
    {
        Task<List<Patient>> GetAllPatientsAsync(int organizationId);

        Task<Patient> GetPatientByIdAsync(int patientId);

        Task AddPatientAsync(Patient patient);

        Task UpdatePatientAsync(Patient patient);

        Task DeletePatientAsync(int patientId);
    }
}