using System;

class Program
{

    public static void Foo(int[,] matrix)
    {
        matrix[0, 0] = 1;
        matrix[1, 1] = 2;
        matrix[2, 2] = 4;
        matrix[3, 3] = 8;
        matrix[4, 4] = 16;
    }

    public static void Main(string[] args)
    {
        int[,] matrix = new int[5, 5];

        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                matrix[i, j] = 0;
                Console.Write($"{matrix[i, j]}\t");
            }
            Console.WriteLine();
        }

        Foo(matrix);

        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                Console.Write($"{matrix[i, j]}\t");
            }
            Console.WriteLine();
        }
    }
}