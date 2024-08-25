namespace MedUnify.HealthPulseAPI.Services.Interface
{
    using MedUnify.Domain.HealthPulse;

    public interface IProgressNoteService
    {
        Task<IEnumerable<ProgressNote>> GetProgressNotesByVisitIdAsync(int visitId);

        Task<ProgressNote> GetProgressNoteByIdAsync(int progressNoteId);

        Task AddProgressNoteAsync(ProgressNote progressNote);

        Task UpdateProgressNoteAsync(ProgressNote progressNote);

        Task DeleteProgressNoteAsync(int progressNoteId);
    }
}