namespace MedUnify.Domain.HealthPulse
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ProgressNote
    {
        public int ProgressNoteId { get; set; }
        public int VisitId { get; set; }
        public string SectionName { get; set; }
        public string SectionText { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public string Height { get; set; }
        public string Weight { get; set; }
        public string Temperature { get; set; }

        // Navigation property
        public Visit? Visit { get; set; }
    }
}