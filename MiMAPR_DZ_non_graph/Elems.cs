using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiMAPR_DZ_non_graph
{
    internal class Katushka : ElemTemp
    {
        private double value = 0;
        public Katushka(int _id, string _real_name, string _programm_name, int _leftSideUzel, int _rightSideUzel, Dictionary<string, double> _parameters) : base(_id, "L", _real_name, _programm_name, _leftSideUzel, _rightSideUzel, _parameters)
        {
            if (parameters.ContainsKey("value"))
            {
                value = parameters["value"];
            }
        }

        public override void AddElemOnTableTest(int x, int y, int cnt, ref double[,] matrix)
        {
            if ((leftSideUzel > 0)&&(rightSideUzel==0))
            {
                matrix[x+leftSideUzel-1, y+leftSideUzel-1] += cnt; // 1/L
            }
            if ((leftSideUzel == 0) && (rightSideUzel > 0))
            {
                matrix[x + rightSideUzel - 1, y + rightSideUzel - 1] += cnt; // 1/L
            }
            if ((leftSideUzel > 0) && (rightSideUzel > 0))
            {
                matrix[x + rightSideUzel - 1, y + rightSideUzel - 1] += cnt;
                matrix[x + leftSideUzel - 1, y + leftSideUzel - 1] += cnt;
                matrix[x + rightSideUzel - 1, y + leftSideUzel - 1] -= cnt;
                matrix[x + leftSideUzel - 1, y + rightSideUzel - 1] -= cnt;
            }
        }

        public override void AddElemOnTable(int x, int y, int cnt, ref double[,] matrix)
        {
            throw new NotImplementedException();
        }

        public override void TestPrint()
        {
            Console.WriteLine($"ID: {id}; name: {name}; val: {value}; real name: {real_name}; programm name: {programm_name}; leftSideUzel: {leftSideUzel}; rightSideUzel: {rightSideUzel}");
        }
    }

    internal class Resistor : ElemTemp
    {
        private double value = 0;
        public Resistor(int _id, string _real_name, string _programm_name, int _leftSideUzel, int _rightSideUzel, Dictionary<string, double> _parameters) : base(_id, "R", _real_name, _programm_name, _leftSideUzel, _rightSideUzel, _parameters)
        {
            if (parameters.ContainsKey("value"))
            {
                value = parameters["value"];
            }
        }

        public override void AddElemOnTable(int x, int y, int cnt, ref double[,] matrix)
        {
            throw new NotImplementedException();
        }

        public override void AddElemOnTableTest(int x, int y, int cnt, ref double[,] matrix)
        {
            if ((leftSideUzel > 0) && (rightSideUzel == 0))
            {
                matrix[x + leftSideUzel - 1, y + leftSideUzel - 1] += cnt; // 1/L
            }
            if ((leftSideUzel == 0) && (rightSideUzel > 0))
            {
                matrix[x + rightSideUzel - 1, y + rightSideUzel - 1] += cnt; // 1/L
            }
            if ((leftSideUzel > 0) && (rightSideUzel > 0))
            {
                matrix[x + rightSideUzel - 1, y + rightSideUzel - 1] += cnt;
                matrix[x + leftSideUzel - 1, y + leftSideUzel - 1] += cnt;
                matrix[x + rightSideUzel - 1, y + leftSideUzel - 1] -= cnt;
                matrix[x + leftSideUzel - 1, y + rightSideUzel - 1] -= cnt;
            }
        }

        public override void TestPrint()
        {
            Console.WriteLine($"ID: {id}; name: {name}; val: {value}; real name: {real_name}; programm name: {programm_name}; leftSideUzel: {leftSideUzel}; rightSideUzel: {rightSideUzel}");
        }
    }

    internal class Condencator : ElemTemp
    {
        private double value = 0;
        public Condencator(int _id, string _real_name, string _programm_name, int _leftSideUzel, int _rightSideUzel, Dictionary<string, double> _parameters) : base(_id, "C", _real_name, _programm_name, _leftSideUzel, _rightSideUzel, _parameters)
        {
            if (parameters.ContainsKey("value"))
            {
                value = parameters["value"];
            }
        }

        public override void AddElemOnTable(int x, int y, int cnt, ref double[,] matrix)
        {
            throw new NotImplementedException();
        }

        public override void AddElemOnTableTest(int x, int y, int cnt, ref double[,] matrix)
        {
            if ((leftSideUzel > 0) && (rightSideUzel == 0))
            {
                matrix[x + leftSideUzel - 1, y + leftSideUzel - 1] += cnt; // 1/L
            }
            if ((leftSideUzel == 0) && (rightSideUzel > 0))
            {
                matrix[x + rightSideUzel - 1, y + rightSideUzel - 1] += cnt; // 1/L
            }
            if ((leftSideUzel > 0) && (rightSideUzel > 0))
            {
                matrix[x + rightSideUzel - 1, y + rightSideUzel - 1] += cnt;
                matrix[x + leftSideUzel - 1, y + leftSideUzel - 1] += cnt;
                matrix[x + rightSideUzel - 1, y + leftSideUzel - 1] -= cnt;
                matrix[x + leftSideUzel - 1, y + rightSideUzel - 1] -= cnt;
            }
        }

        public override void TestPrint()
        {
            Console.WriteLine($"ID: {id}; name: {name}; val: {value}; real name: {real_name}; programm name: {programm_name}; leftSideUzel: {leftSideUzel}; rightSideUzel: {rightSideUzel}");
        }
    }

    internal class EDS : ElemTemp
    {
        private double amplituda = 0;
        private double period = 0;
        private double phase = 0;
        private double value = 0;
        private bool funcFlag = false;
        public EDS(int _id, string _real_name, string _programm_name, int _leftSideUzel, int _rightSideUzel, Dictionary<string, double> _parameters) : base(_id, "E", _real_name, _programm_name, _leftSideUzel, _rightSideUzel, _parameters)
        {
            if (parameters.ContainsKey("value"))
            {
                value = parameters["value"];
            }
            else
            {
                funcFlag = true;
                amplituda = parameters["amplituda"];
                period = parameters["period"];
                phase = parameters["phase"];
            }
        }

        private void SinusoidFunc(double t)
        {
            double pi = Math.PI;
            double frequency = 2 * pi / period;
            value = amplituda * Math.Sin(frequency * t + phase);
            //return value;
        }

        public override void AddElemOnTableTest(int x, int y, int cnt, ref double[,] matrix)
        {
            if ((leftSideUzel > 0) && (rightSideUzel == 0))
            {
                matrix[x + leftSideUzel-1,x + y] += 1; // IE
                matrix[x + y, x + leftSideUzel-1] += 1; // IE
            }
            if ((leftSideUzel == 0) && (rightSideUzel > 0))
            {
                matrix[x + rightSideUzel - 1, x + y] -= 1; // IE
                matrix[x + y, x + rightSideUzel - 1] -= 1; // IE
            }
            if ((leftSideUzel > 0) && (rightSideUzel > 0))
            {
                matrix[x + leftSideUzel - 1, x + y] += 1; // IE
                matrix[x + y, x + leftSideUzel - 1] += 1; // IE
                matrix[x + rightSideUzel - 1, x + y] -= 1; // IE
                matrix[x + y, x + rightSideUzel - 1] -= 1; // IE
            }
        }

        public override void TestPrint()
        {
            Console.WriteLine($"ID: {id}; name: {name}; val: {value}; amp: {amplituda}; per: {period}; real name: {real_name}; programm name: {programm_name}; leftSideUzel: {leftSideUzel}; rightSideUzel: {rightSideUzel}");
        }

        public override void AddElemOnTable(int x, int y, int cnt, ref double[,] matrix)
        {
            throw new NotImplementedException();
        }
    }

    internal class Idiot : ElemTemp
    {
        public Idiot(int _id, string _real_name, string _programm_name, int _leftSideUzel, int _rightSideUzel, Dictionary<string, double> _parameters) : base(_id, "I", _real_name, _programm_name, _leftSideUzel, _rightSideUzel, _parameters)
        {

        }

        public override void AddElemOnTable(int x, int y, int cnt, ref double[,] matrix)
        {
            throw new NotImplementedException();
        }

        public override void AddElemOnTableTest(int x, int y, int cnt, ref double[,] matrix)
        {
            if ((leftSideUzel > 0) && (rightSideUzel == 0))
            {
                matrix[x + leftSideUzel - 1, y + leftSideUzel - 1] += cnt; // 1/L
            }
            if ((leftSideUzel == 0) && (rightSideUzel > 0))
            {
                matrix[x + rightSideUzel - 1, y + rightSideUzel - 1] += cnt; // 1/L
            }
            if ((leftSideUzel > 0) && (rightSideUzel > 0))
            {
                matrix[x + rightSideUzel - 1, y + rightSideUzel - 1] += cnt;
                matrix[x + leftSideUzel - 1, y + leftSideUzel - 1] += cnt;
                matrix[x + rightSideUzel - 1, y + leftSideUzel - 1] -= cnt;
                matrix[x + leftSideUzel - 1, y + rightSideUzel - 1] -= cnt;
            }
        }

        public override void TestPrint()
        {
            Console.WriteLine($"ID: {id}; name: {name}; real name: {real_name}; programm name: {programm_name}; leftSideUzel: {leftSideUzel}; rightSideUzel: {rightSideUzel}");
        }
    }
}
