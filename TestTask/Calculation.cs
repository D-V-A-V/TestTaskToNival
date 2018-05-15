using System;

namespace TestTask
{
    enum Operation
    {
        add = 0,
        multiply,
        divide,
        subtract
    }

    class Calculation
    {
        public double mod;
        public Operation operand;
        public string uid;

        //Конструктор заполняет поля класса значениями полученых из файла
        public Calculation(string _mod, string _operation, string _uid)
        {
            Double.TryParse(_mod, out mod);
            uid = _uid;
            switch (_operation)
            {
                case "add":
                    operand = Operation.add;
                    break;
                case "multiply":
                    operand = Operation.multiply;
                    break;
                case "divide":
                    operand = Operation.divide;
                    break;
                case "subtract":
                    operand = Operation.subtract;
                    break;
            }
        }
        
        //Конструктор заполняет поля класса значениями полученых из файла
        public Calculation(Calculation calculation)
        {
            mod = calculation.mod;
            operand = calculation.operand;
            uid = calculation.uid;
        }
        //Проверка является ли строка операцией
        public static bool IsOperation(string operation)
        {
            switch (operation)
            {
                case "add":
                case "multiply":
                case "divide":
                case "subtract":
                    return true;
                default:
                    return false;
            }
        }
        //Перегрузки операторов
        public static double operator +(double a, Calculation b)
        {
            return a + b.mod;
        }

        public static double operator -(double a, Calculation b)
        {
            return a - b.mod;
        }

        public static double operator *(double a, Calculation b)
        {
            return a * b.mod;
        }

        public static double operator /(double a, Calculation b)
        {
            if (b.mod == 0)
                throw new DivideByZeroException();
                return a / b.mod;
        }
        //Перегрузка ToString()
        public override string ToString()
        {
            switch (operand)
            {
                case Operation.add:
                    return " + " + mod;
                case Operation.multiply:
                    return " * " + mod;
                case Operation.divide:
                    return " / " + mod;
                case Operation.subtract:
                    return " - " + mod;
                default:
                    return "";
            }
            
        }
    }
}