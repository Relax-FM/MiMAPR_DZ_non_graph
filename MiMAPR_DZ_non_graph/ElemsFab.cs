﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Security;
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

        private static double NowTime = 0; // Время сейчас
        // private static double NowStep = 0; // Текущий шаг
        private static double dt = 0; // Текуший шаг
        private static double StopStep = 0; // Величина шага на котором остановилась программа
        private static double FullTime = 0; // Полное время работы программы

        private static double Eps_min = 0;
        private static double Eps_max = 0;
        private static double Iter_count = 0;

        private static double[] vector;
        private static double[,] matrix;

        private static List<double> Phi_now;
        private static List<double> Phi_before;
        private static List<double> Phi_with_dot;
        private static List<double> Phi_extra;
        private static List<double> Dphi_with_dot;
        private static List<double> Phi_integ;
        private static List<double> Phi_integ_before;
        private static List<double> Dphi_integ;
        private static List<double> Dphi;
        private static List<double> Ie;
        private static List<double> Die;

        private static List<ElemTemp> elemList = new List<ElemTemp>();

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

            int k = 0;
            for (int i = 0; i < Ecounter; i++)
            {
                for (int j = k; j < elemList.Count; j++)
                {
                    if (elemList[j].GetName() == "E")
                    {
                        elemList[j].GetEdsOnVector(ref vector, 3 * UselCount);
                        k = j + 1;
                    }
                }
            }

            PrintVector();
        }

        public static void CreateMatrix()
        {
            for (int i = 0; i < UnitMatrixShape; i++) // Единичная матрица
            {
                matrix[i, i] = 1;
            }

            for (int i = 0; i < UselCount; i++)
            {
                matrix[i, i + UnitMatrixShape] = -1.0/dt; // -1/dt
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
                    case "E":
                        elem.AddElemOnMatrix(ref matrix, UnitMatrixShape, UselCount);
                        offset++;
                        break;
                }
            }

            PrintMatrix();
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
            Dphi_with_dot = new List<double>();
            Phi_extra = new List<double>();
            Phi_integ = new List<double>();
            Phi_integ_before = new List<double>();
            Dphi_integ = new List<double>();
            Dphi = new List<double>();
            Ie = new List<double>();
            Die = new List<double>();

            for (int i = 0; i < Ecounter; i++)
            {
                Ie.Add(0);
                Die.Add(0);
            }

            for (int i = 0; i < UselCount; i++)
            {
                Phi_now.Add(0);
                Phi_before.Add(0);
                Phi_with_dot.Add(0);
                Dphi_with_dot.Add(0);
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
            NowTime = start_time == 0 ? 1e-15 : start_time;
            //NowStep = start_step;
            dt = start_step;
            StopStep = start_step;
            FullTime = full_time;
        }

        public static void SetCountingEpsilonSettings(double eps_min, double eps_max, double iter_count = 7)
        {
            Eps_min = eps_min;
            Eps_max = eps_max;
            Iter_count = iter_count;
        }

        private static void Gauss()
        {

        }

        public static void Calculating()
        {
            List<StreamWriter> sw = new List<StreamWriter>();

            for (int i = 0; i < UselCount; i++)
            {
                string filename = $"phi-{i + 1}.txt";
                sw.Add(new StreamWriter(filename));
            }

            // ТУТ ВСЁ РЕШЕНИЕ

            for (int i = 0; i < UselCount; i++)
            {
                sw[i].Close(); // Позакрывали все файлы.
            }

        }

        public static void CalculatingTest()
        {
            Console.WriteLine("Calculating is starting!");
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