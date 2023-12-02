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
        protected string name;
        protected string real_name;
        protected string programm_name;
        protected int leftSideUzel;
        protected int rightSideUzel;
        protected Dictionary<string, double> parameters;


        public ElemTemp(int _id, string _name, string _real_name, string _programm_name, int _leftSideUzel, int _rightSideUzel, Dictionary<string, double> _parameters)
        {
            id = _id;
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

        public abstract void AddElemOnTableTest(int x, int y, int cnt, ref double[,] matrix);
        public abstract void AddElemOnTable(int x, int y, int cnt, ref double[,] matrix);
        public abstract void TestPrint();

    }
}
