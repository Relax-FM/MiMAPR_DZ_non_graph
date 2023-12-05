using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiMAPR_DZ_non_graph
{
    internal abstract class ElemTemp
    {
        protected int id;
        protected int elemId;
        protected string name;
        protected string real_name;
        protected string programm_name;
        protected int leftSideUzel;
        protected int rightSideUzel;
        protected Dictionary<string, double> parameters;

        protected static double NowTime;
        protected static List<double> Phi_now;
        protected static List<double> Phi_integ;
        protected static List<double> Phi_with_dot;
        protected static List<double> Ie;


        public ElemTemp(int _id, int _eid, string _name, string _real_name, string _programm_name, int _leftSideUzel, int _rightSideUzel, Dictionary<string, double> _parameters)
        {
            id = _id;
            elemId = _eid;
            name = _name;
            real_name = _real_name;
            programm_name = _programm_name;
            leftSideUzel = _leftSideUzel;
            rightSideUzel = _rightSideUzel;
            parameters = _parameters;
        }

        public string GetName()
        {
            return name;
        }

        public static void SetPhi(ref List<double> _now, ref List<double> _integ, ref List<double> _with_dot, ref List<double> _Ie, ref double nt)
        {
            Phi_now = _now;
            Phi_integ = _integ;
            Phi_with_dot = _with_dot;
            Ie = _Ie;
            NowTime = nt;
        }

        public virtual void GetEdsOnVectorTest(ref double[] vector, int i, int offset)
        {
            Console.WriteLine("Вот это ты вряд ли увидешь :)");
        }

        public virtual void GetEdsOnVector(ref double[] vector, int offset, double _NowTime)
        {
            Console.WriteLine("Вот это ты вряд ли увидешь :)");
        }

        public abstract void AddElemOnMatrixTest(int x, int y, int cnt, ref double[,] matrix, int offset);
        public abstract void AddElemOnMatrix(ref double[,] matrix, int x, int y);
        public abstract void AddElemOnVectorTest(ref double[] vector, int i, int offset);
        public abstract void AddElemOnVector(ref double[] vector, int offset);
        public abstract void TestPrint();

    }
}
