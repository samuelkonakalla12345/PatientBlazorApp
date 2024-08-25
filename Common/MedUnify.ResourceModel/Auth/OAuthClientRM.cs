namespace MedUnify.ResourceModel.Auth
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class OAuthClientRM
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public int OrganizationId { get; set; }

        public string OrganizationName { get; set; }
    }
}