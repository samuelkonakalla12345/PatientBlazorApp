namespace MedUnify.HealthPulseAPI.Repositories.Concrete
{
    using MedUnify.Domain.HealthPulse;
    using MedUnify.HealthPulseAPI.DbContext;
    using MedUnify.HealthPulseAPI.Repositories.Interface;
    using Microsoft.EntityFrameworkCore;

    public class VisitRepository : IVisitRepository
    {
        private readonly MedUnifyDbContext _context;

        public VisitRepository(MedUnifyDbContext context)
        {
            _context = context;
        }

        public async Task<List<Visit>> GetVisitsByPatientIdAsync(int patientId)
        {
            return await _context.Visits.Where(v => v.PatientId == patientId).Include(x=>x.ProgressNotes).ToListAsync();
        }

        public async Task<Visit> GetVisitByIdAsync(int visitId)
        {
            return await _context.Visits.FindAsync(visitId);
        }

        public async Task AddVisitAsync(Visit visit)
        {
            
            await _context.Visits.AddAsync(visit);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateVisitAsync(Visit visit)
        {
            _context.Visits.Update(visit);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteVisitAsync(int visitId)
        {
            var visit = await _context.Visits.FindAsync(visitId);
            if (visit != null)
            {
                _context.Visits.Remove(visit);
                await _context.SaveChangesAsync();
            }
        }
    }
}