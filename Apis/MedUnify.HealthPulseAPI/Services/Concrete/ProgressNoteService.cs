namespace MedUnify.HealthPulseAPI.Services.Concrete
{
    using MedUnify.Domain.HealthPulse;
    using MedUnify.HealthPulseAPI.Repositories.Interface;
    using MedUnify.HealthPulseAPI.Services.Interface;

    public class ProgressNoteService : IProgressNoteService
    {
        private readonly IProgressNoteRepository _progressNoteRepository;

        public ProgressNoteService(IProgressNoteRepository progressNoteRepository)
        {
            _progressNoteRepository = progressNoteRepository;
        }

        public async Task<IEnumerable<ProgressNote>> GetProgressNotesByVisitIdAsync(int visitId)
        {
            return await _progressNoteRepository.GetProgressNotesByVisitIdAsync(visitId);
        }

        public async Task<ProgressNote> GetProgressNoteByIdAsync(int progressNoteId)
        {
            return await _progressNoteRepository.GetProgressNoteByIdAsync(progressNoteId);
        }

        public async Task AddProgressNoteAsync(ProgressNote progressNote)
        {
            await _progressNoteRepository.AddProgressNoteAsync(progressNote);
        }

        public async Task UpdateProgressNoteAsync(ProgressNote progressNote)
        {
            await _progressNoteRepository.UpdateProgressNoteAsync(progressNote);
        }

        public async Task DeleteProgressNoteAsync(int progressNoteId)
        {
            await _progressNoteRepository.DeleteProgressNoteAsync(progressNoteId);
        }
    }
}