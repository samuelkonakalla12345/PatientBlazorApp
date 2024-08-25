namespace MedUnify.AuthAPI.Repositories
{
    using global::MedUnify.Domain.Auth;
    using MedUnify.AuthAPI.DbContext;
    using Microsoft.EntityFrameworkCore;

    public interface IOAuthClientRepository
    {
        Task<OAuthClient> GetClientByClientIdAsync(string clientId);
    }
}