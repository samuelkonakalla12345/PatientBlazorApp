namespace MedUnify.HealthPulseAPI.Repositories.Concrete
{
    using MedUnify.Domain.HealthPulse;
    using MedUnify.HealthPulseAPI.DbContext;
    using MedUnify.HealthPulseAPI.Repositories.Interface;
    using Microsoft.EntityFrameworkCore;

    public class ProgressNoteRepository : IProgressNoteRepository
    {
        private readonly MedUnifyDbContext _context;

        public ProgressNoteRepository(MedUnifyDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProgressNote>> GetProgressNotesByVisitIdAsync(int visitId)
        {
            return await _context.ProgressNotes.Where(pn => pn.VisitId == visitId).ToListAsync();
        }

        public async Task<ProgressNote> GetProgressNoteByIdAsync(int progressNoteId)
        {
            return await _context.ProgressNotes.FindAsync(progressNoteId);
        }

        public async Task AddProgressNoteAsync(ProgressNote progressNote)
        {
            await _context.ProgressNotes.AddAsync(progressNote);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProgressNoteAsync(ProgressNote progressNote)
        {
            _context.ProgressNotes.Update(progressNote);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProgressNoteAsync(int progressNoteId)
        {
            var progressNote = await _context.ProgressNotes.FindAsync(progressNoteId);
            if (progressNote != null)
            {
                _context.ProgressNotes.Remove(progressNote);
                await _context.SaveChangesAsync();
            }
        }
    }
}