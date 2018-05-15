using System;
using System.Collections.Generic;
using System.Xml;

namespace TestTask
{
    class Calculations
    {
        List<Calculation> calculations = new List<Calculation>();
        //Решение арифметического выражения
        double answer;
        //Количество успешно полученных элементов
        public int successfullyDeserializedElements=0;
        //Наличие ошибок
        bool error = false;
        //Путь к файлу
        public string filename;
        //Сообщение ошибки
        string errorMessage;
        //Метод для передачи его в поток
        public void ReadFile(object filename)
        {
            ReadFile(filename.ToString());
        }
        //Метод считывания элементов из файла и вычисления выражения
        public void ReadFile(string filename)
        {
            this.filename = filename;
            int sizeInputData = 3;
            string[] inputData = new string[sizeInputData];
            bool check;
            //Попытка считать файл по указоному пути
            XmlDocument xDoc = new XmlDocument();
            try
            {
                xDoc.Load(filename);
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                error = true;
                return;
            }

            // Получение корневого элемента "calculations"
            XmlElement xRoot = xDoc.DocumentElement;
            // Обход всех узлов "calculation"
            foreach (XmlNode xnode in xRoot)
            {
                for (int index = 0; index < sizeInputData; index++)
                {
                    inputData[index] = "";
                }
                //Обход всех узлов элемента calculation
                foreach (XmlNode childnode in xnode.ChildNodes)
                {
                    switch (childnode.Attributes.GetNamedItem("name").Value)
                    {
                        case "mod":
                            inputData[0] = childnode.Attributes.GetNamedItem("value").Value;
                            break;
                        case "uid":
                            inputData[2] = childnode.Attributes.GetNamedItem("value").Value;
                            break;
                        case "operand":
                            if (Calculation.IsOperation(childnode.Attributes.GetNamedItem("value").Value))
                                inputData[1] = childnode.Attributes.GetNamedItem("value").Value;
                            break;
                    }
                }
                //Проверка что все поля элемента считаны успешно
                check = true;
                for (int index = 0; index < sizeInputData && check; index++)
                {
                    if (inputData[index] == "")
                        check = false;
                }

                if (check == true)
                {
                    calculations.Add(new Calculation(inputData[0], inputData[1], inputData[2]));
                }
            }
            successfullyDeserializedElements = calculations.Count;
            Calculate();
        }
        //Вычисление полученного выражения
        double Calculate()
        {
            List<Calculation> tempCalculations = new List<Calculation>();
            tempCalculations.Add(new Calculation("0","add",null));
            calculations.ForEach((item) => { tempCalculations.Add(new Calculation(item)); });
            answer = 0;
            for (int index = 0; index < tempCalculations.Count; index++)
            {
                switch(tempCalculations[index].operand)
                {
                    case Operation.multiply:
                        tempCalculations[index - 1].mod *= tempCalculations[index];
                        tempCalculations.RemoveAt(index);
                        index--;
                        break;
                    case Operation.divide:
                        try
                        {
                            tempCalculations[index - 1].mod /= tempCalculations[index];
                            tempCalculations.RemoveAt(index);
                            index--;
                        }
                        catch (DivideByZeroException exp)
                        {
                            return answer = Double.PositiveInfinity;
                        }
                        break;
                }
            }

            foreach (Calculation calculation in tempCalculations)
            {
                switch (calculation.operand)
                {
                    case Operation.add:
                        answer += calculation;
                        break;
                    case Operation.subtract:
                        answer-= calculation;
                        break;
                }
            }
            return answer;
        }

        public override string ToString()
        {
            if (!error)
            {
                string output = "";

                foreach (Calculation calculation in calculations)
                {
                    output += calculation.ToString();
                }
                if (Double.IsInfinity(answer))
                {
                    return 0 + output + " = Divide By Zero";
                }
                else
                    return 0 + output + " = " + answer;
            }
            else
                return errorMessage;
        }
    }
}