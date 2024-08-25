namespace MedUnify.ResourceModel.HealthPulse
{
    using MedUnify.Domain.HealthPulse;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class PatientRM
    {
        public int PatientId { get; set; }

        [Required(ErrorMessage = "FirstName is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName is required.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "State is required.")]
        public string State { get; set; }

        [Required(ErrorMessage = "City is required.")]
        public string City { get; set; }

        [Required(ErrorMessage = "MobileNumber is required.")]
        public string MobileNumber { get; set; }

        public int OrganizationId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<Visit> Visits { get; set; } // No [Required] attribute
        public ICollection<ProgressNote> ProgressNotes { get; set; }
    }
}