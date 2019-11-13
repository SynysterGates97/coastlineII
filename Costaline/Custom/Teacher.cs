using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Costaline.Custome
{
    public class OrdinaryTeacher
    {
        public OrdinaryTeacher()
        {

        }
        protected string _type = "Обычный";
        protected string _isA { get; set; } = "nil";

        protected enum PracticeAttitude
        {
            HALF,
            NONE
        };
        protected int _practiceAttitude { get; set; } = (int)PracticeAttitude.HALF;

        protected enum PassAttitude
        {
            NEGATIVE,
            NONE
        };
        protected int _passAttitude { get; set; } = (int)PassAttitude.NONE;

        protected enum ChitingAttitude
        {
            EXTREMELY_NEGATIVE,
            NEGATIVE,
            NONE
        };
        protected int _chitingAttitude { get; set; } = (int)ChitingAttitude.NEGATIVE;
    }

    public class IndifferentTeacher : OrdinaryTeacher
    {
        public IndifferentTeacher()
        {
            _type = "Безразличный";
            _isA = "Обычный";

            _passAttitude = (int)PassAttitude.NONE;
            _practiceAttitude = (int)PracticeAttitude.NONE;
            _chitingAttitude = (int)ChitingAttitude.NONE;
        }
    }

    public class FairTeacher : OrdinaryTeacher
    {
        public FairTeacher()
        {
            _type = "Справедливый";
            _isA = "Обычный";

            _passAttitude = (int)PassAttitude.NEGATIVE;
            _practiceAttitude = (int)PracticeAttitude.HALF;
            _chitingAttitude = (int)ChitingAttitude.EXTREMELY_NEGATIVE;
        }
    }
}
