namespace MedUnify.ResourceModel.Auth
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class TokenResponseResourceModel
    {
        public string AuthToken { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ExpireOn { get; set; }
    }
}