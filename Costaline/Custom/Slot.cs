using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Costaline
{   
    public class Slot
    {
        public string name;
        public string isA;
        Domain _domain;
        public string value;

        public void AddDomains(Domain domain)
        {
            _domain = domain;       
        }
    }
}
