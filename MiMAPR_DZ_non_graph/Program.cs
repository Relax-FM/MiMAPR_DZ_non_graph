using System;

namespace MiMAPR_DZ_non_graph
{
    class Programm
    {
        static void Main(string[] args)
        {

            ElemsFab.ClearList();

            ElemsFab.AddElem("E", "E20", 1, 0, new Dictionary<string, double>() { { "amplituda", 10.0 }, { "period", 1e-4 }, { "phase", 0.0 } });
            ElemsFab.AddElem("R", "R23", 1, 0, new Dictionary<string, double>() { { "value", 10000.0 } });
            ElemsFab.AddElem("C", "C20", 1, 2, new Dictionary<string, double>() { { "value", 1e-6 } }); // 1e-6
            ElemsFab.AddElem("L", "L16", 1, 2, new Dictionary<string, double>() { { "value", 0.001 } });

            ElemsFab.AddElem("R", "Ru1", 2, 3, new Dictionary<string, double>() { { "value", 1000000.0 } });  // TODO: поспрашивать насчет диода
            ElemsFab.AddElem("C", "Cb1", 2, 3, new Dictionary<string, double>() { { "value", 2e-12 } }); //2e-12
            ElemsFab.AddElem("I", "Id1", 3, 2, new Dictionary<string, double>() { { "it", 1e-12 }, { "mft", 0.026 } });
            ElemsFab.AddElem("R", "Rb1", 3, 4, new Dictionary<string, double>() { { "value", 20.0 } });

            ElemsFab.AddElem("R", "R20", 4, 0, new Dictionary<string, double>() { { "value", 1000.0 } });

            ElemsFab.TestPrintElems();

            ElemsFab.SetUselAndShapeCount(4); // Задаем кол-во узлов
            ElemsFab.SetCountingTimeSettings(0.0, 1e-3, 1e-8); // Временные настройки моделирования
            ElemsFab.SetCountingEpsilonSettings(1e-6, 1e-2); // Критерии останова

            ElemsFab.StartPreparing(); // Выделение памятей и заполнение нулями всех элементов.

            //ElemsFab.CreateMatrix(); // Заполняем матрицу Якоби инфой
            //ElemsFab.CreateVector(); // Заполняем вектор невязок инфой 

            ElemsFab.Calculating_v2();

            //ElemsFab.CalculatingTest(); // Тут вся магия по идее происходить будет когда-нибудь

        }
    }
}