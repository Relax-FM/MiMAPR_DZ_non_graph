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
        public Katushka(int _id, int _eid, string _real_name, string _programm_name, int _leftSideUzel, int _rightSideUzel, Dictionary<string, double> _parameters) : base(_id, _eid, "L", _real_name, _programm_name, _leftSideUzel, _rightSideUzel, _parameters)
        {
            if (parameters.ContainsKey("value"))
            {
                value = parameters["value"];
            }
        }

        public override void AddElemOnMatrix(ref double[,] matrix, int x, int y)
        {
            if ((leftSideUzel > 0) && (rightSideUzel == 0))
            {
                matrix[x + leftSideUzel - 1, y + leftSideUzel - 1] += 1 / value; // 1/L
            }
            if ((leftSideUzel == 0) && (rightSideUzel > 0))
            {
                matrix[x + rightSideUzel - 1, y + rightSideUzel - 1] += 1 / value; // 1/L
            }
            if ((leftSideUzel > 0) && (rightSideUzel > 0))
            {
                matrix[x + rightSideUzel - 1, y + rightSideUzel - 1] += 1 / value;
                matrix[x + leftSideUzel - 1, y + leftSideUzel - 1] += 1 / value;
                matrix[x + rightSideUzel - 1, y + leftSideUzel - 1] -= 1 / value;
                matrix[x + leftSideUzel - 1, y + rightSideUzel - 1] -= 1 / value;
            }
        }

        public override void AddElemOnMatrixTest(int x, int y, int cnt, ref double[,] matrix, int offset = 0)
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

        public override void AddElemOnVector(ref double[] vector, int offset)
        {
            if ((leftSideUzel > 0) && (rightSideUzel == 0))
            {
                vector[leftSideUzel - 1 + offset] += 1 / value * (Phi_integ[leftSideUzel - 1] - 0);
            }
            else if ((leftSideUzel == 0) && (rightSideUzel > 0))
            {
                vector[rightSideUzel - 1 + offset] -= 1 / value * (0 - Phi_integ[rightSideUzel - 1]);
            }
            else if ((leftSideUzel > 0) && (rightSideUzel > 0))
            {
                vector[leftSideUzel - 1 + offset] += 1 / value * (Phi_integ[leftSideUzel - 1] - Phi_integ[rightSideUzel - 1]);
                vector[rightSideUzel - 1 + offset] -= 1 / value * (Phi_integ[leftSideUzel - 1] - Phi_integ[rightSideUzel - 1]);
            }
        }

        public override void AddElemOnVectorTest(ref double[] vector, int i, int offset)
        {
            if (leftSideUzel > 0)
            {
                vector[leftSideUzel - 1 + offset] += id;
            }
            if (rightSideUzel > 0)
            {
                vector[rightSideUzel - 1 + offset] -= id;
            }
        }

        public override void TestPrint()
        {
            Console.WriteLine($"ID: {id}; name: {name}; val: {value}; real name: {real_name}; programm name: {programm_name}; leftSideUzel: {leftSideUzel}; rightSideUzel: {rightSideUzel}");
        }
    }

    internal class Resistor : ElemTemp
    {
        private double value = 0;
        public Resistor(int _id, int _eid, string _real_name, string _programm_name, int _leftSideUzel, int _rightSideUzel, Dictionary<string, double> _parameters) : base(_id, _eid, "R", _real_name, _programm_name, _leftSideUzel, _rightSideUzel, _parameters)
        {
            if (parameters.ContainsKey("value"))
            {
                value = parameters["value"];
            }
        }

        public override void AddElemOnMatrix(ref double[,] matrix, int x, int y)
        {
            if ((leftSideUzel > 0) && (rightSideUzel == 0))
            {
                matrix[x + leftSideUzel - 1, y + leftSideUzel - 1] += 1 / value; // 1/L
            }
            if ((leftSideUzel == 0) && (rightSideUzel > 0))
            {
                matrix[x + rightSideUzel - 1, y + rightSideUzel - 1] += 1 / value; // 1/L
            }
            if ((leftSideUzel > 0) && (rightSideUzel > 0))
            {
                matrix[x + rightSideUzel - 1, y + rightSideUzel - 1] += 1 / value;
                matrix[x + leftSideUzel - 1, y + leftSideUzel - 1] += 1 / value;
                matrix[x + rightSideUzel - 1, y + leftSideUzel - 1] -= 1 / value;
                matrix[x + leftSideUzel - 1, y + rightSideUzel - 1] -= 1 / value;
            }
        }

        public override void AddElemOnMatrixTest(int x, int y, int cnt, ref double[,] matrix, int offset = 0)
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

        public override void AddElemOnVector(ref double[] vector, int offset)
        {
            if ((leftSideUzel > 0) && (rightSideUzel == 0))
            {
                vector[leftSideUzel - 1 + offset] += 1 / value * (Phi_now[leftSideUzel - 1] - 0);
            }
            else if ((leftSideUzel == 0) && (rightSideUzel > 0))
            {
                vector[rightSideUzel - 1 + offset] -= 1 / value * (0 - Phi_now[rightSideUzel - 1]);
            }
            else if ((leftSideUzel > 0) && (rightSideUzel > 0))
            {
                vector[leftSideUzel - 1 + offset] += 1 / value * (Phi_now[leftSideUzel - 1] - Phi_now[rightSideUzel - 1]);
                vector[rightSideUzel - 1 + offset] -= 1 / value * (Phi_now[leftSideUzel - 1] - Phi_now[rightSideUzel - 1]);
            }
        }

        public override void AddElemOnVectorTest(ref double[] vector, int i, int offset)
        {
            if (leftSideUzel > 0)
            {
                vector[leftSideUzel - 1 + offset] += id;
            }
            if (rightSideUzel > 0)
            {
                vector[rightSideUzel - 1 + offset] -= id;
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
        public Condencator(int _id, int _eid, string _real_name, string _programm_name, int _leftSideUzel, int _rightSideUzel, Dictionary<string, double> _parameters) : base(_id, _eid, "C", _real_name, _programm_name, _leftSideUzel, _rightSideUzel, _parameters)
        {
            if (parameters.ContainsKey("value"))
            {
                value = parameters["value"];
            }
        }

        public override void AddElemOnMatrix(ref double[,] matrix, int x, int y)
        {
            if ((leftSideUzel > 0) && (rightSideUzel == 0))
            {
                matrix[x + leftSideUzel - 1, y + leftSideUzel - 1] += value; // 1/L
            }
            if ((leftSideUzel == 0) && (rightSideUzel > 0))
            {
                matrix[x + rightSideUzel - 1, y + rightSideUzel - 1] += value; // 1/L
            }
            if ((leftSideUzel > 0) && (rightSideUzel > 0))
            {
                matrix[x + rightSideUzel - 1, y + rightSideUzel - 1] += value;
                matrix[x + leftSideUzel - 1, y + leftSideUzel - 1] += value;
                matrix[x + rightSideUzel - 1, y + leftSideUzel - 1] -= value;
                matrix[x + leftSideUzel - 1, y + rightSideUzel - 1] -= value;
            }
        }

        public override void AddElemOnMatrixTest(int x, int y, int cnt, ref double[,] matrix, int offset = 0)
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

        public override void AddElemOnVector(ref double[] vector, int offset)
        {
            if ((leftSideUzel > 0) && (rightSideUzel == 0))
            {
                vector[leftSideUzel - 1 + offset] += value * (Phi_with_dot[leftSideUzel - 1] - 0);
            }
            else if ((leftSideUzel == 0) && (rightSideUzel > 0))
            {
                vector[rightSideUzel - 1 + offset] -= value * (0 - Phi_with_dot[rightSideUzel - 1]);
            }
            else if ((leftSideUzel > 0) && (rightSideUzel > 0))
            {
                vector[leftSideUzel - 1 + offset] += value * (Phi_with_dot[leftSideUzel - 1] - Phi_with_dot[rightSideUzel - 1]);
                vector[rightSideUzel - 1 + offset] -= value * (Phi_with_dot[leftSideUzel - 1] - Phi_with_dot[rightSideUzel - 1]);
            }
        }

        public override void AddElemOnVectorTest(ref double[] vector, int i, int offset)
        {
            if (leftSideUzel > 0)
            {
                vector[leftSideUzel - 1 + offset] += id;
            }
            if (rightSideUzel > 0)
            {
                vector[rightSideUzel - 1 + offset] -= id;
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
        public EDS(int _id, int _eid, string _real_name, string _programm_name, int _leftSideUzel, int _rightSideUzel, Dictionary<string, double> _parameters) : base(_id, _eid, "E", _real_name, _programm_name, _leftSideUzel, _rightSideUzel, _parameters)
        {
            if (parameters.ContainsKey("value"))
            {
                funcFlag = false;
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

        private double SinusoidFunc(double t)
        {
            double pi = Math.PI;
            double frequency = 2 * pi / period;
            double res = amplituda * Math.Sin(frequency * t + phase);
            return res;
        }

        public override void GetEdsOnVectorTest(ref double[] vector, int i, int offset)
        {
            if ((leftSideUzel > 0) && (rightSideUzel == 0))
            {
                vector[offset + i] = id;
            }
            else if ((leftSideUzel == 0) && (rightSideUzel > 0))
            {
                vector[offset + i] = -id;
            }
            else
            {
                Console.WriteLine("Тут я пока хз как делать");
            }
        }

        public override void GetEdsOnVector(ref double[] vector, int offset, double _NowTime)
        {
            NowTime = _NowTime;
            double E = funcFlag ? SinusoidFunc(NowTime) : value;
            if ((leftSideUzel > 0) && (rightSideUzel == 0))
            {
                vector[offset + elemId - 1] = (Phi_now[leftSideUzel - 1] - 0) - E;
            }
            else if ((leftSideUzel == 0) && (rightSideUzel > 0))
            {
                vector[offset + elemId - 1] = (0 - Phi_now[rightSideUzel - 1]) - E;
            }
            else if ((leftSideUzel > 0) && (rightSideUzel > 0))
            {
                vector[offset + elemId - 1] = (Phi_now[leftSideUzel - 1] - Phi_now[rightSideUzel - 1]) - E;
            }
        }

        public override void AddElemOnMatrixTest(int x, int y, int cnt, ref double[,] matrix, int offset)
        {
            if ((leftSideUzel > 0) && (rightSideUzel == 0))
            {
                matrix[x + leftSideUzel - 1, x + y + elemId - 1] += 1; // IE
                matrix[x + y + elemId - 1, x + leftSideUzel - 1] += 1; // IE
            }
            if ((leftSideUzel == 0) && (rightSideUzel > 0))
            {
                matrix[x + rightSideUzel - 1, x + y + elemId - 1] -= 1; // IE
                matrix[x + y + elemId - 1, x + rightSideUzel - 1] -= 1; // IE
            }
            if ((leftSideUzel > 0) && (rightSideUzel > 0))
            {
                matrix[x + leftSideUzel - 1, x + y + elemId - 1] += 1; // IE
                matrix[x + y + elemId - 1, x + leftSideUzel - 1] += 1; // IE
                matrix[x + rightSideUzel - 1, x + y + elemId - 1] -= 1; // IE
                matrix[x + y + elemId - 1, x + rightSideUzel - 1] -= 1; // IE
            }
        }

        public override void TestPrint()
        {
            Console.WriteLine($"ID: {id}; name: {name}; val: {value}; amp: {amplituda}; per: {period}; real name: {real_name}; programm name: {programm_name}; leftSideUzel: {leftSideUzel}; rightSideUzel: {rightSideUzel}");
        }

        public override void AddElemOnMatrix(ref double[,] matrix, int x, int y)
        {
            if ((leftSideUzel > 0) && (rightSideUzel == 0))
            {
                matrix[x + leftSideUzel - 1, x + y + elemId - 1] += 1; // IE
                matrix[x + y + elemId - 1, x + leftSideUzel - 1] += 1; // IE
            }
            if ((leftSideUzel == 0) && (rightSideUzel > 0))
            {
                matrix[x + rightSideUzel - 1, x + y + elemId - 1] -= 1; // IE
                matrix[x + y + elemId - 1, x + rightSideUzel - 1] -= 1; // IE
            }
            if ((leftSideUzel > 0) && (rightSideUzel > 0))
            {
                matrix[x + leftSideUzel - 1, x + y + elemId - 1] += 1; // IE
                matrix[x + y + elemId - 1, x + leftSideUzel - 1] += 1; // IE
                matrix[x + rightSideUzel - 1, x + y + elemId - 1] -= 1; // IE
                matrix[x + y + elemId - 1, x + rightSideUzel - 1] -= 1; // IE
            }
        }

        public override void AddElemOnVectorTest(ref double[] vector, int i, int offset)
        {
            if (leftSideUzel > 0)
            {
                vector[leftSideUzel - 1 + offset] += id;
            }
            if (rightSideUzel > 0)
            {
                vector[rightSideUzel - 1 + offset] -= id;
            }
        }

        public override void AddElemOnVector(ref double[] vector, int offset)
        {
            if ((leftSideUzel > 0) && (rightSideUzel == 0))
            {
                vector[leftSideUzel - 1 + offset] += Ie[elemId - 1];
            }
            else if ((leftSideUzel == 0) && (rightSideUzel > 0))
            {
                vector[rightSideUzel - 1 + offset] -= Ie[elemId - 1];
            }
            else if ((leftSideUzel > 0) && (rightSideUzel > 0))
            {
                vector[leftSideUzel - 1 + offset] += Ie[elemId - 1];
                vector[rightSideUzel - 1 + offset] -= Ie[elemId - 1];
            }
        }
    }

    internal class Idiot : ElemTemp
    {
        private double It = 0.0;
        private double Mft = 0.0;
        public Idiot(int _id, int _eid, string _real_name, string _programm_name, int _leftSideUzel, int _rightSideUzel, Dictionary<string, double> _parameters) : base(_id, _eid, "I", _real_name, _programm_name, _leftSideUzel, _rightSideUzel, _parameters)
        {
            if (parameters.ContainsKey("it"))
            {
                It = parameters["it"];
                Mft = parameters["mft"];
            }
        }

        public double CountI()
        {
            double res;
            double x1 = leftSideUzel == 0 ? 0 : Phi_now[leftSideUzel - 1];
            double x2 = rightSideUzel == 0 ? 0 : Phi_now[rightSideUzel - 1];

            res = It * (Math.Exp((x1 - x2) / Mft) - 1);

            return res;
        }

        public override void AddElemOnMatrix(ref double[,] matrix, int x, int y)
        {
            double x1 = leftSideUzel == 0 ? 0 : Phi_now[leftSideUzel - 1];
            double x2 = rightSideUzel == 0 ? 0 : Phi_now[rightSideUzel - 1];
            double val = It / Mft * (Math.Exp((x1 - x2) / Mft));

            if ((leftSideUzel > 0) && (rightSideUzel == 0))
            {
                matrix[x + leftSideUzel - 1, y + leftSideUzel - 1] += val; // 1/L
            }
            if ((leftSideUzel == 0) && (rightSideUzel > 0))
            {
                matrix[x + rightSideUzel - 1, y + rightSideUzel - 1] += val; // 1/L
            }
            if ((leftSideUzel > 0) && (rightSideUzel > 0))
            {
                matrix[x + rightSideUzel - 1, y + rightSideUzel - 1] += val;
                matrix[x + leftSideUzel - 1, y + leftSideUzel - 1] += val;
                matrix[x + rightSideUzel - 1, y + leftSideUzel - 1] -= val;
                matrix[x + leftSideUzel - 1, y + rightSideUzel - 1] -= val;
            }
        }

        public override void AddElemOnMatrixTest(int x, int y, int cnt, ref double[,] matrix, int offset = 0)
        {
            throw new NotImplementedException("Куда лезешь, тебе сюда не надо");
        }

        public override void AddElemOnVector(ref double[] vector, int offset)
        {
            if ((leftSideUzel > 0) && (rightSideUzel == 0))
            {
                vector[leftSideUzel - 1 + offset] += CountI();
            }
            else if ((leftSideUzel == 0) && (rightSideUzel > 0))
            {
                vector[rightSideUzel - 1 + offset] -= CountI();
            }
            else if ((leftSideUzel > 0) && (rightSideUzel > 0))
            {
                vector[leftSideUzel - 1 + offset] += CountI();
                vector[rightSideUzel - 1 + offset] -= CountI();
            }
        }

        public override void AddElemOnVectorTest(ref double[] vector, int i, int offset)
        {

            if (leftSideUzel > 0)
            {
                vector[leftSideUzel - 1 + offset] += id;
            }
            if (rightSideUzel > 0)
            {
                vector[rightSideUzel - 1 + offset] -= id;
            }

            //if (leftSideUzel - 1 == i)
            //{
            //    vector[i + offset] += id;
            //}
            //else if (rightSideUzel - 1 == i)
            //{
            //    vector[i + offset] -= id;
            //}
        }

        public override void TestPrint()
        {
            Console.WriteLine($"ID: {id}; name: {name}; It: {It}; Mft: {Mft}; real name: {real_name}; programm name: {programm_name}; leftSideUzel: {leftSideUzel}; rightSideUzel: {rightSideUzel}");
        }
    }
}
