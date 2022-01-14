using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteAspire.Modelos
{
    public class AuditLog
    {
        public int Id { get; set; }
        public string Action { get; set; }
        public string TableName { get; set; }
        public string OldValues { get; set; }
        public string NewValues { get; set; }
        public DateTime MachineDate { get; set; }
        public DateTime SystemDate { get; set; }
        public string UserName { get; set; }
        public string OfficeId { get; set; }
    }
}
