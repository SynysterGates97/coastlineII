using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Costaline.Custom
{
    public class Frame//TODO: вынести в отдельный модуль, ибо он выше остальных
    {

        private string _name = "NULL";
        private int _markIndicator = -1;
        private string _isA = "NULL";
        private List<Frame> _valuesDomain = new List<Frame>();
        public virtual List<object> domainValues() { return null; }
        public virtual void questionProcedure() { }

        //Todo: Домен допустимых значений

        public Frame()
        {

        }
    }
}
