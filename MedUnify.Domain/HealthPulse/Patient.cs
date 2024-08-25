namespace MedUnify.Domain.HealthPulse
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Patient
    {
        public int PatientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string City { get; set; }

        public string MobileNumber { get; set; }
        public int OrganizationId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        //public ICollection<Visit> Visits { get; set; } // No [Required] attribute
        //public ICollection<ProgressNote> ProgressNotes { get; set; }
    }
}