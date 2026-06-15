using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yarane.Domain.Entities
{
    public class ClientConfig : BaseEntity
    {
        public string ConsumerName { get; set; }
        public string TerminalNumber { get; set; }
        public int TerminalType { get; set; }
        public long AcceptorCode { get; set; }
    }
  

  
}
