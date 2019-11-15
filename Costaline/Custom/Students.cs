using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Costaline
{
    public class OrdinaryStudent:Frame
    {
        public OrdinaryStudent()
        {
            _name = "Обычный студент";

        }
        protected string _type { get; set; } = "Обычный";
        protected string _isA { get; set; } = "nil";

        protected enum cheatingValues
        {
            ALWAYS,
            SOMETIMES,
            NEVER
        };
        protected int _cheating { get; set; } = (int)cheatingValues.SOMETIMES;

        protected enum practPointsValues
        {
            MARK2,//двойка <40
            MARK3,
            MARK4,
            MARK5
        };
        protected int _practPoints { get; set; } = (int)practPointsValues.MARK4;

        protected enum examPreparationValues
        {
            NONE,
            BAD,
            DECENT,
            COMPLETELY
        };
        protected int _examPreparation { get; set; } = (int)examPreparationValues.DECENT;

        protected enum attendanceValues
        {
            MORE_THAN_80,
            IN_60_80,
            IN_40_60,
            LESS_THAN_40
        };
        protected int _attendance { get; set; } = (int)attendanceValues.IN_60_80;
    }

    public class MediocreStudent : OrdinaryStudent
    {
        public MediocreStudent()
        {
            _isA = "Обычный";
            _type = "Посредственый";

            _cheating = (int)cheatingValues.ALWAYS;// посмотри точно списывает ли всегда посредственый студент
            _attendance = (int)attendanceValues.IN_40_60;
            _examPreparation = (int)examPreparationValues.BAD;
            _practPoints = (int)practPointsValues.MARK3;

        }
    }

    public class ExcellentStudent : OrdinaryStudent
    {
        public ExcellentStudent()
        {
            _isA = "Обычный";
            _type = "Посредственый";

            _cheating = (int)cheatingValues.NEVER;
            _attendance = (int)attendanceValues.MORE_THAN_80;
            _examPreparation = (int)examPreparationValues.COMPLETELY;
            _practPoints = (int)practPointsValues.MARK5;
        }
    }

    public class IdleStudent : MediocreStudent
    {
        public IdleStudent()
        {
            _isA = "Посредственый";
            _type = "Пофигист";

            _attendance = (int)attendanceValues.LESS_THAN_40;
            _examPreparation = (int)examPreparationValues.NONE;
            _practPoints = (int)practPointsValues.MARK2;
        }

    }
}
