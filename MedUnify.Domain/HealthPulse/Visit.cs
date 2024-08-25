namespace MedUnify.Domain.HealthPulse
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Visit
    {
        public int VisitId { get; set; }
        public int PatientId { get; set; }
        public DateTime VisitDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public string Status { get; set; }

        // Navigation properties
        public Patient? Patient { get; set; }

        public ICollection<ProgressNote>? ProgressNotes { get; set; }
    }
}