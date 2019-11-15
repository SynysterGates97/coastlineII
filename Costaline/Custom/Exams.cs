using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Costaline
{
    public class Exam5
    {
        public Exam5()
        {

        }
        private OrdinaryStudent _student { get; set; }
        private OrdinaryTeacher _teacher { get; set; }
        protected string _isA { get; set; } = "nil";

        protected enum roomTypesValues
        {
            SUITABLE_FOR_TEACHING,
            NOT_SUITABLE_FOR_TEACHING
        };
        protected int _roomType { get; set; }

        protected enum SubjectDifficultsValues
        {
            SIMPLE,
            HARD
        };
        protected int _subjectDifficultValues { get; set; }

        private int _subjectDifficult { get; set; }
    }

    public class Exam4 : Exam5
    {
        public Exam4()
        {

        }

    }

    public class Exam3 : Exam5
    {
        public Exam3()
        {

        }

    }

    public class Exam2 : Exam5
    {
        public Exam2()
        {

        }

    }
}
