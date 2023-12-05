using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Dynamic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace MiMAPR_DZ_non_graph
{
    internal class ElemsFab
    {
        // private static double maxElem = 0;

        private static int IDcounter = 0;
        private static int Rcounter = 0;
        private static int Ccounter = 0;
        private static int Lcounter = 0;
        private static int Ecounter = 0;
        private static int Icounter = 0;
        private static int UselCount = 0;
        private static int UnitMatrixShape = 0;
        private static int ShapeCount = 0;

        private static double NowTime = 0; // Время сейчас
        // private static double NowStep = 0; // Текущий шаг
        private static double dt = 0; // Текуший шаг
        // private static double dt_before = 0;
        private static double StopStep = 0; // Величина шага на котором остановилась программа
        private static double FullTime = 0; // Полное время работы программы

        private static double Eps_min = 0;
        private static double Eps_max = 0;
        private static double Deviation = 0;
        private static double Iter_count = 0;

        private static double[] vector;
        private static double[,] matrix;

        private static List<double> Phi_now;
        private static List<double> Phi_before;
        private static List<double> Phi_with_dot;
        private static List<double> Phi_extra;
        private static List<double> Phi_with_dot_before;
        private static List<double> Phi_integ;
        private static List<double> Phi_integ_before;
        private static List<double> Dphi_integ; // MB delete
        private static List<double> Dphi; // MB delete
        private static List<double> Ie;
        private static List<double> Ie_before;

        private static List<ElemTemp> elemList = new List<ElemTemp>();

        public static int Calculating_v2()
        {
            StreamWriter[] sw = new StreamWriter[UselCount];

            for (int i = 0; i < UselCount; i++)
            {
                string filename = $"phi-{i + 1}.txt";
                sw[i] = new StreamWriter(filename);
            }
            Console.WriteLine("Открыл файлы норм");

            StopStep = dt;
            // ClearMatrixAndVector();
            while (NowTime <= FullTime)
            {
                double dt_before = dt; // dt_before - delta_t; dt - step_t; StopStep - step_t_last
                bool cnt_flag = true;
                int iteration = 0;
                while (cnt_flag)
                {
                    ClearTogether();
                    CreateTogether();

                    if (Gauss() == false)
                    {
                        Console.WriteLine("Gausu hana ;(");
                        return 1;
                    }

                    Summing_with_vector();
                    iteration++;

                    if (MaxOrEps())
                    {
                        // Console.WriteLine("Zdes'Tut");
                        cnt_flag = true;
                    }
                    else
                    {
                        cnt_flag = false;
                    }

                    if (iteration > Iter_count && cnt_flag == true)
                    {
                        iteration = 0;
                        // Console.WriteLine("HeH");
                        dt_before /= 2;
                        Summing_with_before();
                    }

                }
                // Тут типа Ньютон закончился (поэтерировали на шаге пора и новый шаг ботрачить)  // dt_before - delta_t; dt - step_t; StopStep - step_t_last

                double[] dev_loc = new double[UselCount];

                for (int i = 0; i < UselCount; i++)
                {
                    dev_loc[i] = Math.Abs(dt / (dt + StopStep) * ((Phi_now[i] - Phi_before[i]) - dt / StopStep * (Phi_before[i] - Phi_extra[i])));
                }

                if (CheckDev_loc(ref dev_loc))
                {
                    dt /= 2;
                    //Console.WriteLine("DIV!  ");
                    Summing_with_before();
                }
                else
                {
                    // Console.WriteLine("SUCCS  !");
                    Summing_before_extra();
                    StopStep = dt_before;

                    for (int i = 0; i < UselCount; i++)
                    {
                        sw[i].WriteLine($"{NowTime:F9} {Phi_now[i]:F9}");
                    }
                    NowTime += dt_before;

                    if (CheckDev_loc(ref dev_loc, false))
                    {
                        dt *= 2; //dt
                    }

                }
            }

            Console.WriteLine("Результаты сохранены в файлы типа phi-1.txt");
            for (int i = 0; i < UselCount; i++)
            {
                sw[i].Close(); // Позакрывали все файлы.
            }
            return 0;

        }

        private static void Summing_before_extra()
        {
            for (int i = 0; i < UselCount; i++)
            {
                Phi_extra[i] = Phi_before[i];
                Phi_before[i] = Phi_now[i];
                Phi_integ_before[i] = Phi_integ[i];
                Phi_with_dot_before[i] = Phi_with_dot[i];
            }
            for (int i = 3 * UselCount; i < ShapeCount; i++)
            {
                Ie_before[i - 3 * UselCount] = Ie[i - 3 * UselCount];
            }

        }

        private static bool CheckDev_loc(ref double[] dev_loc,bool flag = true)
        {
            bool res = false;
            if (flag)
            {
                for (int i = 0; i < UselCount; i++)
                {
                    if (dev_loc[i] > Eps_max)
                    {
                        res = true;
                    }
                }
            }
            else
            {
                res = true;
                for (int i = 0; i < UselCount; i++)
                {
                    if (dev_loc[i] >= Eps_min)
                    {
                        res = false;
                    }
                }
            }
            return res;
        }

        private static void Summing_with_vector()
        {
            for (int i = 0; i < UselCount; i++)
            {
                Phi_with_dot[i] = Phi_with_dot[i] + vector[i]; // фи с точкой + дельта фи с точкой
                Phi_integ[i] = Phi_integ[i] + vector[i + UselCount]; // интеграл от фи плюс дельта интеграл от фи
                Phi_now[i] = Phi_now[i] + vector[i + (2 * UselCount)]; // Фи плюс дельта фи
            }

            for (int i = 3 * UselCount; i < ShapeCount; i++)
            {
                Ie[i - 3 * UselCount] = Ie[i - 3 * UselCount] + vector[i];
            }
        }

        private static void Summing_with_before()
        {
            for (int i = 0; i < UselCount; i++)
            {
                Phi_with_dot[i] = Phi_with_dot_before[i]; // фи с точкой + дельта фи с точкой
                Phi_integ[i] = Phi_integ_before[i]; // интеграл от фи плюс дельта интеграл от фи
                Phi_now[i] = Phi_before[i];// Фи плюс дельта фи
            }

            for (int i = 3 * UselCount; i < ShapeCount; i++)
            {
                Ie[i - 3 * UselCount] = Ie_before[i - 3 * UselCount];
            }
        }

        public static int Calculating()
        {
            StreamWriter[] sw = new StreamWriter[UselCount];

            for (int i = 0; i < UselCount; i++)
            {
                string filename = $"phi-{i + 1}.txt";
                sw[i] = new StreamWriter(filename);
            }
            Console.WriteLine("Открыл файлы норм");

            // ТУТ ВСЁ РЕШЕНИЕ
            // TODO: дописать солвер
            // ТУТ ВСЁ РЕШЕНИЕ

            while (NowTime <= FullTime)
            {
                ClearTogether();
                CreateTogether();
                if (Gauss())
                {
                    Summing();
                    Console.WriteLine($"Даже до сюда дошел {Phi_now[0]}");
                    for (int i = 0; i < UselCount; i++)
                    {
                        sw[i].WriteLine($"{NowTime:F9} {Phi_now[i]:F9}");
                    }
                }
                else
                {
                    Console.WriteLine("Suetaaaa");
                    return 1;
                }
                NowTime += dt;
            }


            for (int i = 0; i < UselCount; i++)
            {
                sw[i].Close(); // Позакрывали все файлы.
            }
            return 0;

        }

        private static void Summing()
        {
            for (int i = 0; i < UselCount; i++)
            {
                Phi_with_dot[i] = Phi_with_dot[i] + vector[i]; // фи с точкой + дельта фи с точкой
                Phi_integ_before[i] = Phi_integ[i]; // интеграл от фи на нынешнем шаге стал интегралом на предыдущем
                Phi_integ[i] = Phi_integ[i] + vector[i + UselCount]; // интеграл от фи плюс дельта интеграл от фи
                Phi_extra[i] = Phi_before[i]; // фи прошлое стало фи пазапрошлым
                Phi_before[i] = Phi_now[i]; // фи текущее стало фи предыдущим
                Phi_now[i] = Phi_now[i] + vector[i + (2 * UselCount)]; // Фи плюс дельта фи
            }

            for (int i = 3*UselCount; i < ShapeCount; i++)
            {
                Ie[i-3*UselCount] = Ie[i-3*UselCount] + vector[i];
            }
        }

        public static void ClearMatrix()
        {
            for (int i = 0; i < ShapeCount; i++)
            {
                for (int j = 0; j < ShapeCount; j++)
                {
                    matrix[i, j] = 0;
                }
            }
        }

        public static void ClearVector()
        {
            for (int i = 0; i < ShapeCount; i++)
            {
                vector[i] = 0;
            }
        }

        public static void ClearTogether()
        {
            ClearMatrix();
            ClearVector();
        }

        public static void CreateVector()
        {
            for (int i = 0; i < UselCount; i++)
            {
                vector[i] = Phi_with_dot[i] - (Phi_now[i] - Phi_before[i]) / dt;
            }
            for (int i = 0; i < UselCount; i++)
            {
                vector[i + UselCount] = Phi_integ[i] - (Phi_integ_before[i] + Phi_now[i] * dt);
            }
            
            foreach (var elem in elemList)
            {
                elem.AddElemOnVector(ref vector, 2 * UselCount);
            }

            //int k = 0;
            
            for (int j = 0; j < elemList.Count; j++)
            {
                if (elemList[j].GetName() == "E")
                {
                     elemList[j].GetEdsOnVector(ref vector, 3 * UselCount, NowTime);
                    //k = j + 1;
                } 
            }

            MinusVector();
            //PrintVector();
        }

        public static void CreateMatrix()
        {
            for (int i = 0; i < UnitMatrixShape; i++) // Единичная матрица
            {
                matrix[i, i] = 1;
            }

            for (int i = 0; i < UselCount; i++)
            {
                matrix[i, i + UnitMatrixShape] = -1.0 / dt; // -1/dt
                matrix[i + UselCount, i + UnitMatrixShape] = -dt; // -dt
            }

            int cnt = 0;
            int offset = 0;
            foreach (var elem in elemList)
            {
                cnt++;
                switch (elem.GetName())
                {
                    case "C":
                        elem.AddElemOnMatrix(ref matrix, UnitMatrixShape, 0);
                        break;
                    case "L":
                        elem.AddElemOnMatrix(ref matrix, UnitMatrixShape, UselCount);
                        break;
                    case "R":
                        elem.AddElemOnMatrix(ref matrix, UnitMatrixShape, UnitMatrixShape);
                        break;
                    case "I":
                        elem.AddElemOnMatrix(ref matrix, UnitMatrixShape, UnitMatrixShape);
                        break;
                    case "E":
                        elem.AddElemOnMatrix(ref matrix, UnitMatrixShape, UselCount);
                        offset++;
                        break;
                }
            }

            //PrintMatrix();
        }

        public static void CreateTogether()
        {
            CreateMatrix();
            CreateVector();
        }

        private static bool MaxOrEps() // если true - vector > deviation; false - все vector[i] < deviation
        {
            for (int i = 0; i < vector.Length; i++)
            {
                if (vector[i] > Deviation)
                {
                    return true;
                }
            }
            return false;
        }

        private static void MinusVector()
        {
            for (int i = 0; i < vector.Length; i++)
            {
                vector[i] *= -1;
            }
        }

        public static void CreateTestVector()
        {
            for (int i = 0; i < UselCount; i++)
            {
                vector[i] = 0;
            }
            for (int i = 0; i < UselCount; i++)
            {
                vector[i + UselCount] = 1;
            }
            

            foreach (var elem in elemList)
            {
                elem.AddElemOnVectorTest(ref vector, 0, 2 * UselCount);
            }


            int k = 0;
            for (int i = 0; i < Ecounter; i++)
            {
                for (int j = k; j < elemList.Count; j++)
                {
                    if (elemList[j].GetName() == "E")
                    {
                        elemList[j].GetEdsOnVectorTest(ref vector, i, 3*UselCount);
                        k = j+1;
                    }
                }
            }
            PrintVector();
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
            int offset = 0;
            foreach (var elem in elemList)
            {
                cnt++;
                switch (elem.GetName())
                {
                    case "C":
                        elem.AddElemOnMatrixTest(UnitMatrixShape, 0, cnt, ref matrix, 0);
                        break;
                    case "L":
                        elem.AddElemOnMatrixTest(UnitMatrixShape, UselCount, cnt, ref matrix, 0);
                        break;
                    case "R":
                        elem.AddElemOnMatrixTest(UnitMatrixShape, UnitMatrixShape, cnt, ref matrix, 0);
                        break;
                    case "E":
                        elem.AddElemOnMatrixTest(UnitMatrixShape, UselCount, cnt, ref matrix, offset);
                        offset++;
                        break;
                }
            }

            PrintTestMatrix();
        }

        public static void StartPreparing()
        {
            ClearMatrixAndVector();
            SetStartValues();
            ElemTemp.SetPhi(ref Phi_now, ref Phi_integ, ref Phi_with_dot, ref Ie, ref NowTime);
        }

        public static void SetStartValues()
        {
            Phi_now = new List<double>();
            Phi_before = new List<double>();
            Phi_with_dot = new List<double>();
            Phi_with_dot_before = new List<double>();
            Phi_extra = new List<double>();
            Phi_integ = new List<double>();
            Phi_integ_before = new List<double>();
            Dphi_integ = new List<double>();
            Dphi = new List<double>();
            Ie = new List<double>();
            Ie_before = new List<double>();

            for (int i = 0; i < Ecounter; i++)
            {
                Ie.Add(0);
                Ie_before.Add(0);
            }

            for (int i = 0; i < UselCount; i++)
            {
                Phi_now.Add(0);
                Phi_before.Add(0);
                Phi_with_dot.Add(0);
                Phi_with_dot_before.Add(0);
                Phi_extra.Add(0);
                Phi_integ.Add(0);
                Phi_integ_before.Add(0);
                Dphi_integ.Add(0);
                Dphi.Add(0);
                //Ie.Add(0);
                //Die.Add(0);
                Console.WriteLine($"Phi_{i + 1} now is {Phi_now[i]}");
            }
        }

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
            PrintVector("Start");
        }

        public static void PrintVector(string s = "Now")
        {
            Console.WriteLine($"\n{s} Vector: ");
            for (int i = 0; i < ShapeCount; i++)
            {
                Console.Write($"{vector[i]}");
                Console.WriteLine();
            }
        }

        public static void PrintMatrix(string s = "Now")
        {
            Console.WriteLine($"\n{s} Matrix: ");
            for (int i = 0; i < ShapeCount; i++)
            {
                for (int j = 0; j < ShapeCount; j++)
                {
                    Console.Write($"{matrix[i, j]}\t"); // ($"{matrix[i, j]:F2}\t") чтобы форматировать.
                }
                Console.WriteLine();
            }
        }

        public static void PrintTestMatrix()
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

        public static void AddElem(string _name, string _real_name, int _leftSideUzel, int _rightSideUzel, Dictionary<string, double> _parameters)
        {
            IDcounter++;
            switch (_name)
            {
                case "R":
                    Rcounter++;
                    elemList.Add(new Resistor(IDcounter, Rcounter,_real_name, new String($"{_name}{Rcounter}"),_leftSideUzel, _rightSideUzel, _parameters));
                    break;
                case "C":
                    Ccounter++;
                    elemList.Add(new Condencator(IDcounter, Ccounter,_real_name, new String($"{_name}{Ccounter}"), _leftSideUzel, _rightSideUzel, _parameters));
                    break;
                case "L":
                    Lcounter++;
                    elemList.Add(new Katushka(IDcounter, Lcounter,_real_name, new String($"{_name}{Lcounter}"), _leftSideUzel, _rightSideUzel, _parameters));
                    break;
                case "E":
                    Ecounter++;
                    elemList.Add(new EDS(IDcounter, Ecounter,_real_name, new String($"{_name}{Ecounter}"), _leftSideUzel, _rightSideUzel, _parameters));
                    break;
                case "I":
                    Icounter++;
                    elemList.Add(new Idiot(IDcounter, Icounter,_real_name, new String($"{_name}{Icounter}"), _leftSideUzel, _rightSideUzel, _parameters));
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
            NowTime = start_time;//start_time == 0 ? 1e-15 : start_time; //TODO: Узнать насчет начального времени // start_time; //
            // NowStep = start_step;
            // dt_before = start_step;
            dt = start_step;
            // StopStep = start_step;
            FullTime = full_time;
        }

        public static void SetCountingEpsilonSettings(double eps_min, double eps_max, double deviation = 1e-3,double iter_count = 7)
        {
            Eps_min = eps_min;
            Eps_max = eps_max;
            Deviation = deviation;
            Iter_count = iter_count;
        }

        private static bool Gauss()
        {
            /// <summary>Computes the solution of a linear equation system with Gauss method.</summary>
            /// <param name="matrix">
            /// The system of linear equations matrix[row, col] where (rows == cols).
            /// </param>
            /// <param name="vector">
            /// The answers of system of linear equations vector[row] where (rows == cols).
            /// </param>
            /// <returns>Returns true - if matrix can be soluted by Gauss. Return false - if couldn't</returns>
            int i, j, k;
            double diagElem = 0;
            for (i = 0; i < vector.Length; i++) // Прямой проход
            {
                diagElem = matrix[i, i];
                if (diagElem == 0)
                {
                    return false; // Вырожденная матрица(
                }

                for (j = i; j < vector.Length; j++)
                {
                    matrix[i, j] /= diagElem;
                }
                vector[i] /= diagElem;

                for (k = i + 1; k < vector.Length; k++)
                {
                    diagElem = matrix[k, i];
                    for (j = i; j < vector.Length; j++)
                    {
                        matrix[k,j] -= matrix[i,j] * diagElem;
                    }
                    vector[k] -= vector[i] * diagElem;
                }
            }

            for (i = vector.Length - 2; i >= 0 ; i--) // Обратка
            {
                for (j = i+1; j < vector.Length; j++)
                {
                    vector[i] -= matrix[i, j] * vector[j];
                }
            }

            return true;

        }

        public static void CalculatingTest()
        {
            Console.WriteLine("Start calculating");
            if (Gauss())
            {
                PrintMatrix();
                PrintVector();
            }
        }

        public static void TestPrintElems()
        {
            foreach (var elem in elemList)
            {
                elem.TestPrint();
            }
        }
    }
}
