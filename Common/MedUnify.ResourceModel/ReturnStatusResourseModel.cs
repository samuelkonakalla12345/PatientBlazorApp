namespace MedUnify.ResourceModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ReturnStatusResourseModel
    {
        public string Status { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;
    }
}