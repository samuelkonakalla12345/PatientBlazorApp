namespace MedUnify.AuthAPI.Repositories.Concrete
{
    using global::MedUnify.Domain.Auth;
    using MedUnify.AuthAPI.DbContext;
    using Microsoft.EntityFrameworkCore;

    public class OAuthClientRepository : IOAuthClientRepository
    {
        private readonly MedUnifyDbContext _context;

        public OAuthClientRepository(MedUnifyDbContext context)
        {
            _context = context;
        }

        public async Task<OAuthClient> GetClientByClientIdAsync(string clientId)
        {
            return await _context.Set<OAuthClient>()
                .SingleOrDefaultAsync(c => c.ClientId == clientId);
        }
    }
}