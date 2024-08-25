namespace MedUnify.HealthPulseAPI.Services.Concrete
{
    using MedUnify.Domain.HealthPulse;
    using MedUnify.HealthPulseAPI.Repositories.Interface;
    using MedUnify.HealthPulseAPI.Services.Interface;

    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;

        public PatientService(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public async Task<List<Patient>> GetAllPatientsAsync(int organizationId)
        {
            return await _patientRepository.GetAllPatientsAsync(organizationId);
        }

        public async Task<Patient> GetPatientByIdAsync(int patientId)
        {
            return await _patientRepository.GetPatientByIdAsync(patientId);
        }

        public async Task AddPatientAsync(Patient patient)
        {
            await _patientRepository.AddPatientAsync(patient);
        }

        public async Task UpdatePatientAsync(Patient patient)
        {
            await _patientRepository.UpdatePatientAsync(patient);
        }

        public async Task DeletePatientAsync(int patientId)
        {
            await _patientRepository.DeletePatientAsync(patientId);
        }
    }
}