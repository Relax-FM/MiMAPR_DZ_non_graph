using System;
using System.Numerics;

class Program
{
    private static bool Gauss(ref double[,] matrix, ref double[] vector)
    {
        // TODO: дописать Гаусса
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
                    matrix[k, j] -= matrix[i, j] * diagElem;
                }
                vector[k] -= vector[i] * diagElem;
            }
        }

        for (i = vector.Length - 2; i >= 0; i--)
        {
            for (j = i + 1; j < vector.Length; j++)
            {
                vector[i] -= matrix[i, j] * vector[j];
            }
        }

        return true;

    }


    public static void Main(string[] args)
    {
        /*double[,] matrix = new double[5, 5] {
        //    { 5, 4, 3, 2, 1},
        //    { 4, 3, 2, 1, 1},
        //    { 2, 3, 1, 2, 1},
        //    { 1, 3, 1, 1, 1},
        //    { 1, 1, 1, 2, 1}
        //};
        //double[] vector = new double[5]
        //{
        //    10, 9, 8, 7, 6
        //};

        //for (int i = 0; i < 5; i++)
        //{
        //    for (int j = 0; j < 5; j++)
        //    {
        //        Console.Write($"{matrix[i, j]}\t");
        //    }
        //    Console.WriteLine();
        //}
        //Console.WriteLine();
        //Console.WriteLine();
        //for (int i = 0; i < 5; i++)
        //{
        //    Console.Write($"{vector[i]}\t");
        //    Console.WriteLine();
        //}
        //Console.WriteLine();


        //Console.WriteLine("Start calculating");
        //if (Gauss(ref matrix,ref vector))
        //{
        //    for (int i = 0; i < 5; i++)
        //    {
        //        for (int j = 0; j < 5; j++)
        //        {
        //            Console.Write($"{matrix[i, j]}\t");
        //        }
        //        Console.WriteLine();
        //    }
        //    Console.WriteLine();
        //    Console.WriteLine();
        //    for (int i = 0; i < 5; i++)
        //    {
        //        Console.Write($"{vector[i]}\t");
        //        Console.WriteLine();
        //    }
        //    Console.WriteLine();
        //}*/

        int N = 5; // Количество файлов
        StreamWriter[] sw = new StreamWriter[N];


        for (int i = 0; i < N; i++)
        {
            string fileName = $"file{i}.txt"; // Имя файла (например, file1.txt, file2.txt и т.д.)
            sw[i] = new StreamWriter(fileName);
            
        }

        for (int i = 0; i < N; i++)
        {
            sw[i].WriteLine("12345");
        }

        for (int i = 0; i < N; i++)
        {
            sw[i].WriteLine("12345");
        }

        for (int i = 0; i < N; i++)
        {
            sw[i].WriteLine("12345");
        }

        for (int i = 0; i < N; i++)
        {
            sw[i].WriteLine("HUI HUI PIZDA PIZDA");
        }

        for (int i = 0; i < N; i++)
        {
            sw[i].Close();
        }

    }
}