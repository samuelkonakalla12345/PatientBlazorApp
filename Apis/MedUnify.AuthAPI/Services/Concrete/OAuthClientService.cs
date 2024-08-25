namespace MedUnify.AuthAPI.Services.Concrete
{
    using MedUnify.AuthAPI.Repositories;
    using MedUnify.AuthAPI.Services.Interface;
    using MedUnify.Domain.Auth;

    public class OAuthClientService : IOAuthClientService
    {
        private readonly IOAuthClientRepository _oAuthClientRepository;

        public OAuthClientService(IOAuthClientRepository oAuthClientRepository)
        {
            _oAuthClientRepository = oAuthClientRepository;
        }

        public async Task<OAuthClient> GetClientByClientIdAsync(string clientId)
        {
            // Here you can add any business logic if needed before accessing the repository
            return await _oAuthClientRepository.GetClientByClientIdAsync(clientId);
        }
    }
}