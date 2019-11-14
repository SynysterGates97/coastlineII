using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Costaline.Custom
{
    public class Frame//TODO: вынести в отдельный модуль, ибо он выше остальных
    {

        private string _name;
        private int _markIndicator;
        public virtual void questionProcedure() { }
        //Todo: Домен допустимых значений

        public Frame()
        {

        }
    }
}
