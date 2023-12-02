using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiMAPR_DZ_non_graph
{
    internal class ElemsFab
    {
        private static int IDcounter = 0;
        private static int Rcounter = 0;
        private static int Ccounter = 0;
        private static int Lcounter = 0;
        private static int Ecounter = 0;
        private static int Icounter = 0;
        private static int UselCount = 0;
        private static int UnitMatrixShape = 0;
        private static int ShapeCount = 0;
        private static double NowTime = 0;
        private static double NowStep = 0;
        private static double FullTime = 0;
        private static double Eps1 = 0;
        private static double Eps2 = 0;
        private static double Eps3 = 0;
        private static double[,] matrix;
        private static double[] vector;
        private static List<double> Phi_now;
        private static List<double> Phi_after;
        private static List<ElemTemp> elemList = new List<ElemTemp>();

        public static void ClearList()
        {
            elemList.Clear();
        }

        public static void ClearMatrixAndVector()
        {
            matrix = new double[ShapeCount, ShapeCount];
            vector = new double[ShapeCount];
            Console.WriteLine("\nStart Matrix: ");
            for (int i = 0; i < ShapeCount; i++)
            {
                for (int j = 0; j < ShapeCount; j++)
                {
                    matrix[i, j] = 0;
                    Console.Write($"{matrix[i, j]}\t");
                }
                Console.WriteLine();
                vector[i] = 0;
            }
            PrintVector();
        }

        public static void PrintVector()
        {
            Console.WriteLine("\nNow Vector: ");
            for (int i = 0; i < ShapeCount; i++)
            {
                Console.Write($"{vector[i]}");
                Console.WriteLine();
            }
        }

        public static void PrintMatrix()
        {
            Console.WriteLine("\nNow Matrix: ");
            for (int i = 0; i < ShapeCount; i++)
            {
                for (int j = 0; j < ShapeCount; j++)
                {
                    if (matrix[i, j] == -111111.0)
                    {
                        Console.Write("-1/dt\t");
                    }
                    else if (matrix[i, j] == -100000.0)
                    {
                        Console.Write("-dt\t");
                    }
                    else
                        Console.Write($"{matrix[i, j]}\t");
                    
                }
                Console.WriteLine();
            }
        }

        public static void CreateTestVector()
        {

        }


            public static void CreateTestMatrix()
        {
            for (int i = 0; i < UnitMatrixShape; i++) // Единичная матрица
            {
                matrix[i, i] = 1;
            }

            for (int i = 0; i < UselCount; i++)
            {
                matrix[i, i + UnitMatrixShape] = -100000; // -1/dt
                matrix[i + UselCount, i + UnitMatrixShape] = -111111; // -dt
            }

            int cnt = 0;
            foreach (var elem in elemList)
            {
                cnt++;
                switch (elem.GetName())
                {
                    case "C":
                        elem.AddElemOnTableTest(UnitMatrixShape, 0, cnt, ref matrix);
                        break;
                    case "L":
                        elem.AddElemOnTableTest(UnitMatrixShape, UselCount, cnt, ref matrix);
                        break;
                    case "R":
                        elem.AddElemOnTableTest(UnitMatrixShape, UnitMatrixShape, cnt, ref matrix);
                        break;
                    case "E":
                        elem.AddElemOnTableTest(UnitMatrixShape, UselCount, cnt, ref matrix);
                        break;
                }
            }

            PrintMatrix();
        }

        public static void AddElem(string _name, string _real_name, int _leftSideUzel, int _rightSideUzel, Dictionary<string, double> _parameters)
        {
            IDcounter++;
            switch (_name)
            {
                case "R":
                    Rcounter++;
                    elemList.Add(new Resistor(IDcounter, _real_name, new String($"{_name}{Rcounter}"),_leftSideUzel, _rightSideUzel, _parameters));
                    break;
                case "C":
                    Ccounter++;
                    elemList.Add(new Condencator(IDcounter, _real_name, new String($"{_name}{Ccounter}"), _leftSideUzel, _rightSideUzel, _parameters));
                    break;
                case "L":
                    Lcounter++;
                    elemList.Add(new Katushka(IDcounter, _real_name, new String($"{_name}{Lcounter}"), _leftSideUzel, _rightSideUzel, _parameters));
                    break;
                case "E":
                    Ecounter++;
                    elemList.Add(new EDS(IDcounter, _real_name, new String($"{_name}{Ecounter}"), _leftSideUzel, _rightSideUzel, _parameters));
                    break;
                case "I":
                    Icounter++;
                    elemList.Add(new Idiot(IDcounter, _real_name, new String($"{_name}{Icounter}"), _leftSideUzel, _rightSideUzel, _parameters));
                    break;
            }
        }

        public static void SetUselAndShapeCount(int uselCount)
        {
            UselCount = uselCount;
            ShapeCount = UselCount*3+Ecounter;
            UnitMatrixShape = UselCount * 2;
        }

        public static void SetCountingTimeSettings(double start_time,double full_time, double start_step) 
        {
            NowTime = start_time;
            NowStep = start_step;
            FullTime = full_time;
        }

        public static void SetCountingEpsilonSettings(double eps1, double eps2, double eps3=0)
        {
            Eps1 = eps1;
            Eps2 = eps2;
            Eps3 = eps3;
        }

        public static void SetStartPhi()
        {
            Phi_now = new List<double>();
            Phi_after = new List<double>();
            for (int i = 0; i < UselCount; i++)
            {
                Phi_now.Add(0);
                Phi_after.Add(0);
                Console.WriteLine($"Phi_{i+1} now is {Phi_now[i]}");
            }
        }

        public static void Calculating()
        {
            Console.WriteLine("Calculating is starting!");
        }

        public static void TestPrint()
        {
            foreach (var elem in elemList)
            {
                elem.TestPrint();
            }
        }
    }
}
