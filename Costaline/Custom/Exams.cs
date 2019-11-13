using System;

namespace Сostaline
{
    public class Exam
    {
        public Exam()
        {

        }
        private Student _student;
        private Teacher _teacher;

        enum roomTypesValues
        {
            SUITABLE_FOR_TEACHING,
            NOT_SUITABLE_FOR_TEACHING
        };
        private int _roomType
        {
            get;
            set;
        }

        enum SubjectDifficultsValues
        {
            SUITABLE_FOR_TEACHING,
            NOT_SUITABLE_FOR_TEACHING
        };
        private int _subjectDifficult
        {
            get;
            set;
        }
    }

    public class Exam2 : Exam
    {
        Exam a2;

        public Exam2()
        {

        }

    }

    public class Exam3 : Exam
    {
        Exam a3;

        public Exam3()
        {

        }

    }

    public class Exam4 : Exam
    {
        Exam a4;

        public Exam2()
        {

        }

    }

    public class Exam5 : Exam
    {
        Exam a5;

        public Exam2()
        {

        }

    }
}