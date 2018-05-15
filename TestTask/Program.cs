using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace TestTask
{
    class Program
    {
        static void Main(string[] args)
        {
            //Считывание путь к папке где находятся xml файлы.
            //Если путь не корректен или файлов нет, об этом будет сообщено
            Console.WriteLine("Input Path:");
            try
            {
                string[] files = Directory.GetFiles(@Console.ReadLine(), "*.xml", SearchOption.TopDirectoryOnly);
                TestTask(files);
                Console.Read();
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.Read();
            }  
        }

        static void TestTask(string[] files)
        {
            //проверка н аналичие в папке нужных файлов.
            if (files.Count() == 0)
            {
                Console.WriteLine("Don`t found any files *.xml");
                return;
            }
            //Массив арифметических выражений полученых из файлов
            Calculations[] calculations = new Calculations[files.Count()];

            //Максимальное число успешно десериализированых элементов в одном файле
            int maxSuccessfullyDeserializedElements = 0;

            //Счетчик времени отображения времени исполнения
            Stopwatch sw = new Stopwatch();
            sw.Start();
            //Массив потоков
            Thread[] threads = new Thread[files.Count()];
            
            //Запуск десериализации и подсчета значения выражения в потоках
            for (int i = 0; i < files.Count(); i++)
            {
                calculations[i] = new Calculations();
                threads[i] = new Thread(calculations[i].ReadFile);
                threads[i].Start(files[i]);

            }

            //Остановка главного потока, пока остальные не завершатся
            foreach (Thread thread in threads)
            {
                thread.Join();
            }

            //Поиск maxSuccessfullyDeserializedElements и вывод полученых результатов
            foreach (Calculations calcul in calculations)
            {
                if (calcul.successfullyDeserializedElements > maxSuccessfullyDeserializedElements)
                    maxSuccessfullyDeserializedElements = calcul.successfullyDeserializedElements;
                Console.WriteLine("File :" + calcul.filename);
                Console.WriteLine("Result :" + calcul.ToString());
            }
            sw.Stop();

            Console.WriteLine("Time in run: {0} millisec.", (sw.ElapsedMilliseconds).ToString());
            Console.WriteLine("Max successfully deserialized Elements {0} in {1}", maxSuccessfullyDeserializedElements,
                calculations.Where(x => x.successfullyDeserializedElements == maxSuccessfullyDeserializedElements).First().filename);
        }
    }
}
