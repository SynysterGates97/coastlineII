using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Costaline
{
    class Domain
    {
        public string Name { get; set; }// ToDo сделать более осмысленое имя. Пока оно определяет для какого слота используется. Напиться.
        public List<string> Values { get; set; }

        public Domain()
        {
            Values = new List<string>();
        }

    }
}
