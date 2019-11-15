using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Costaline
{
    public class Frame//TODO: вынести в отдельный модуль, ибо он выше остальных
    {
        public struct FrameInfo
        {
            public string name;
            public object frame;
        }
        protected string _name { get; set; } = "NULL";

        protected string _isA { get; set; } = "NULL";

        protected List<FrameInfo> frameInfos = new List<FrameInfo>();

        public virtual void questionProcedure() { }

        private int _markIndicator = -1;

        //Todo: Домен допустимых значений

        public Frame()
        {
            int index = frameInfos.Count();
            for 
            if(frameInfos[0].name == "Студент")
            {
                //Делаем, все что хотим
            }
        }
    }
}
