using System;

namespace coastline
{
    public class Frame//TODO: вынести в отдельный модуль, ибо он выше остальных
    {

        private string _name;
        private int _markIndicator;
        virtual void questionProcedure();
        //Todo: Домен допустимых значений

        public Frame()
        {

        }
    }

    public class Student
    {
        private string _type = "Обычный";

        enum CheatingValues
        {
            ALWAYS,
            SOMETIMES,
            NEVER
        };
        private int _cheating = CheatingValues.SOMETIMES;

        enum practPointsValues
        {
            MARK2,//двойка <40
            MARK3,
            MARK4,
            MARK5
        };
        private int _practPoints = practPointsValues.MARK4;

        enum examPreparationValues
        {
            NONE,
            BAD,
            DECENT,
            COMPLETELY
        };
        private int _examPreparation = examPreparationValues.DECENT;

        enum attendanceValues
        {
            MORE_THAN_80,
            IN_60_80,
            IN_20_60,
            LESS_THAN_20
        };
        private int _attendance = attendanceValues.IN_60_80;

        public Student()
        {

        }
    }

    public class Teacher
    {
        public Teacher()
        {

        }
    }

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
}
