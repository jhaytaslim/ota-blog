using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Common;
namespace Domain.Entities
{
    public class Session : AuditableEntity
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public Guid EmployeeId { get; set; }
        public string StaffId { get; set; }
        public string FeatureList { get; set; }
    }
}
