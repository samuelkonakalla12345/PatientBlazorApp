namespace MedUnify.HealthPulseAPI.Infrastructure.Handlers
{
    using System.Security.Claims;

    public interface IOrganizationHandler
    {
        int GetOrganizationIdFromToken(ClaimsPrincipal user);
    }

    public class OrganizationHandler : IOrganizationHandler
    {
        public int GetOrganizationIdFromToken(ClaimsPrincipal user)
        {
            // Extract the OrganizationId from the token claims
            var organizationIdClaim = user.FindFirst("OrganizationId");

            if (organizationIdClaim == null)
            {
                throw new UnauthorizedAccessException("OrganizationId not found in token.");
            }

            // Convert the organizationIdClaim value to an integer
            if (!int.TryParse(organizationIdClaim.Value, out int organizationId))
            {
                throw new ArgumentException("Invalid OrganizationId in token.");
            }

            return organizationId;
        }
    }
}