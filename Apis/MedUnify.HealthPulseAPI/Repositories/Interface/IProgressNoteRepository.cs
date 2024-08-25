namespace MedUnify.HealthPulseAPI.Repositories.Interface
{
    using MedUnify.Domain.HealthPulse;

    public interface IProgressNoteRepository
    {
        Task<List<ProgressNote>> GetProgressNotesByVisitIdAsync(int visitId);

        Task<ProgressNote> GetProgressNoteByIdAsync(int progressNoteId);

        Task AddProgressNoteAsync(ProgressNote progressNote);

        Task UpdateProgressNoteAsync(ProgressNote progressNote);

        Task DeleteProgressNoteAsync(int progressNoteId);
    }
}