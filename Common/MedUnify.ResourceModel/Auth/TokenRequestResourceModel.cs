namespace MedUnify.ResourceModel.Auth
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class TokenRequestResourceModel
    {
        [Required(ErrorMessage = "ClientId is required.")]
        public string ClientId { get; set; }

        [Required(ErrorMessage = "ClientSecret is required.")]
        public string ClientSecret { get; set; }
    }
}