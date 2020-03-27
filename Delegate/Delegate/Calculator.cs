using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegate
{
    class Calculator
    {
        /// <summary>
        /// Инициализация делегата.
        /// </summary>
        /// <param name="a"> Первое слагаемое/множитель/... </param>
        /// <param name="b"> Второй аргумент в выражении. </param>
        /// <returns></returns>
        public delegate double MathOperation(double a, double b);

        /// <summary>
        /// Метод сложения.
        /// </summary>
        /// <param name="a"> Первое слагаемое. </param>
        /// <param name="b"> Второе слагаемое. </param>
        /// <returns></returns>
        public static double Plus(double a, double b)
        {
            return a + b;
        }

        /// <summary>
        /// Метод умножения.
        /// </summary>
        /// <param name="a"> Первый множитель. </param>
        /// <param name="b"> Второй множитель. </param>
        /// <returns></returns>
        public static double Mult(double a, double b)
        {
            return a * b;
        }

        /// <summary>
        /// Метод деления.
        /// </summary>
        /// <param name="a"> Делимое. </param>
        /// <param name="b"> Делитель. </param>
        /// <returns></returns>
        public static double Divide(double a, double b)
        {
            return a / b;
        }

        /// <summary>
        /// Метод вычитания.
        /// </summary>
        /// <param name="a"> Уменьшаемое. </param>
        /// <param name="b"> Вычитаемое. </param>
        /// <returns></returns>
        public static double Minus(double a, double b)
        {
            return a - b;
        }

        /// <summary>
        /// Метод возведения в степень.
        /// </summary>
        /// <param name="a"> Возводимое в степень. </param>
        /// <param name="b"> Степень. </param>
        /// <returns></returns>
        public static double Stepen(double a, double b)
        {
            return Math.Pow(a, b);
        }

        /// <summary>
        /// Метод, которое сплитит строку и возвращает результат выражения. 
        /// </summary>
        /// <param name="expr"> Строка с ппримером. </param>
        /// <returns></returns>
        public double Calculate(string expr)
        {
            try
            {
                string[] problem = expr.Split(' ');
                double a = double.Parse(problem[0].ToString());
                double b = double.Parse(problem[2].ToString());
                if (double.IsNaN(Math.Round(operations[problem[1]](a, b), 3)))
                {
                    throw new Exception("Результат не является числом.");
                }
                else if (problem[1] == "/" && b == 0)
                {
                    throw new Exception("Деление на 0.");
                }
                else if (!operations.ContainsKey(problem[1]))
                {
                    throw new Exception("Некорректно введенная операция.");
                }
                return operations[problem[1]](a, b);
            }
            catch (Exception ex)
            {
                // Вызов события. Можно вызвать без точки, но так безопаснее.
                ErrorNotification?.Invoke(ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// Метод проверки настоящего ответа и ответа в предложенном для провкерки файле.
        /// </summary>
        /// <param name="realAns"> Результат, вычисляемый программой. </param>
        /// <param name="expr"> Результат в файле. </param>
        /// <returns></returns>
        public string Check(string realAns, string expr)
        {
            if (realAns == expr) return "OK";
            else return "Error";
        }

        /// <summary>
        /// Событие, ревгируещее на возникновение исключений.
        /// </summary>
        public event Program.ErrorNotificationType ErrorNotification;

        /// <summary>
        /// Словарь с операциями. Калькулятор, по факт.
        /// </summary>
        public static Dictionary<string, MathOperation> operations = new Dictionary<string, MathOperation>(5);

        /// <summary>
        /// Публичный конструктор для создания объекта калькулятора у которого 
        /// будет доступ к методам, в которых участвует словарь.
        /// </summary>
        public Calculator()
        {
            /*АЛЬТЕРНАТИВА: СДЕЛАТЬ ЧЕРЕЗ АНОНИМНЫЕ МЕТОДЫ ИЛИ ЛЯМБДА_ВЫРАЖЕНИЯ.*/
            MathOperation delPlus = Plus;
            MathOperation delMinus = Minus;
            MathOperation delMult = Mult;
            MathOperation delDiv = Divide;
            MathOperation delStepen = Stepen;

            // Передача делегатов в словарь. 
            operations.Add("*", delMult);
            operations.Add("/", delDiv);
            operations.Add("+", delPlus);
            operations.Add("-", delMinus);
            operations.Add("^", delStepen);

        }

    }
}
