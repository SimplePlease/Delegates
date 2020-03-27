using System;
using System.IO; // - для работы с файлами.
using System.Security; // - для обработки эксепшена.
using System.Collections.Generic; // - для работы со словарем. 
/// <summary>
/// Вахитова Диана БПИ199. 
/// </summary>

namespace Delegate
{
    class Program
    {
        /// <summary>
        /// Константы с названием путей для чтения и записи файлов. Все располагается в директории с решением.
        /// </summary>
        public const string input1 = "../../../expressions.txt";
        public const string output1 = "../../../answers.txt";
        public const string input2 = "../../../expressions_checker.txt";
        public const string output2 = "../../../results.txt";

        /// <summary>
        /// Инициализация делегата.
        /// </summary>
        /// <param name="a"> Первое слагаемое/множитель/... </param>
        /// <param name="b"> Второй аргумент в выражении. </param>
        /// <returns></returns>
        public delegate double MathOperation(double a, double b);

        /// <summary>
        /// Объявление словаря со строкой-ключом и делегатами-значениями.
        /// </summary>
        public static Dictionary<string, MathOperation> operations = new Dictionary<string, MathOperation>(5);

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
        /// Метод, которое сплитит строку и возвращаем результат выражения. 
        /// </summary>
        /// <param name="expr"> Строка с ппримером. </param>
        /// <returns></returns>
        public static double Calculate(string expr)
        {
            string[] problem = expr.Split(' ');
            double a = double.Parse(problem[0].ToString());
            double b = double.Parse(problem[2].ToString());
            return operations[problem[1]](a, b);
        }

        /// <summary>
        /// Метод проверки настоящего ответа и ответа в предложенном для провкерки файле.
        /// </summary>
        /// <param name="realAns"> Результат, вычисляемый программой. </param>
        /// <param name="expr"> Результат в файле. </param>
        /// <returns></returns>
        public static string Check(string realAns, string expr)
        {
            if (realAns == expr) return "OK";
            else return "Error";
        }

        /// <summary>
        /// Сверяем ответы с ответами из файла с правильными ответами. Записываем результат ещё в один файл.
        /// Запишем суммарное количество ошибок.
        /// </summary>
        /// <param>  </param>
        public static void CheckAnswers()
        {
            try
            {
                // Счетчик ошибок.
                int mistakes = 0;
                // Чтение из файлов в массивы строк. 
                string[] secondDoc = File.ReadAllLines(input2);
                string[] answers = File.ReadAllLines(output1);

                // Сравниваем отвеы, записываем в файл results.
                for (int i = 0; i < answers.Length; i++)
                {
                    try
                    {
                        if (Check(answers[i], secondDoc[i]) == "Error") mistakes++; // Увелечение значения счетчика.

                        // Запись в файл.
                        File.AppendAllText(output2, Check(answers[i], secondDoc[i]) + Environment.NewLine);
                    }
                    catch (Exception)
                    {
                    }
                }
                File.AppendAllText(output2, "Всего ошибок: " + mistakes);
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (SecurityException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void Main(string[] args)
        {
            // Передача методов в делегаты. 
            // Альтернативное решение - сделать через анонимные методы.
            /*MathOperation delTrue;
            operations.Add("0", delTrue = (a, b) => a + b);
            MathOperation delTry = (a, b) => a + b;
            Console.WriteLine(delTry(3, 5));
            Console.WriteLine(operations["0"](4, 8));*/
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
            do
            {
                Console.WriteLine("Чекайте файлы и подождите предложение о повторе решения.");
                try
                {
                    // Если программу будут перезапускать, то удалять файлы с результатами после каждого запуска.
                    if (File.Exists(output1))
                    {
                        File.Delete(output1);
                    }
                    if (File.Exists(output2))
                    {
                        File.Delete(output2);
                    }

                    // Чтение из файлов в массивы строк. 
                    string[] firstDoc = File.ReadAllLines(input1);
                    string[] secondDoc = File.ReadAllLines(input2);

                    for (int i = 0; i < firstDoc.Length; i++)
                    {
                        try
                        {
                            // Вычиление результата выражения из первого файла. 
                            double answer = Calculate(firstDoc[i]);

                            // Запись в файл.
                            File.AppendAllText(output1, answer.ToString("f3") + Environment.NewLine);
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Входная строка имела неверный формат.");
                        }
                    }
                    CheckAnswers();
                }
                // Ексепшены. 
                catch (IOException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (UnauthorizedAccessException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (SecurityException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                Console.WriteLine("Нажмите Enter, чтобы повторить.");
            } while (Console.ReadKey().Key == ConsoleKey.Enter);

        }
    }
}
