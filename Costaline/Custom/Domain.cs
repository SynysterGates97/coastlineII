using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Costaline
{
    public class Domain
    {
        public string name { get; set; }
        public List<string> values { get; set; }

        public Domain()
        {
            values = new List<string>();
        }
        // не должно быть возможности добавить домен
    }
}
