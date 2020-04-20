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
        /// Делегат тип для создания ивента.
        /// </summary>
        /// <param name="messege"> Сообщение о типе ошибки.</param>
        public delegate void ErrorNotificationType(string messege);

        /// <summary>
        /// Константы с названием путей для чтения и записи файлов. Все располагается в директории с решением.
        /// </summary>
        public const string input1 = "../../../expressions.txt";
        public const string output1 = "../../../answers.txt";
        public const string input2 = "../../../expressions_checker.txt";
        public const string output2 = "../../../results.txt";

        /// <summary>
        /// Метод для вывода на консоль ошибок. Обработчик события.
        /// </summary>
        /// <param name="message"> Сообщение на консоли о типе ошибки + дата.</param>
        public static void ConsoleErrorHandler(string message)
        {
            Console.WriteLine(message + " " + DateTime.Now);
        }

        /// <summary>
        /// Обработчик события. Для каждого типа сохраняет свое значение в файл.
        /// </summary>
        /// <param name="message"></param>
        public static void ResultErrorHandler(string message)
        {
            try
            {
                if (message == "Результат не является числом.")
                {
                    File.AppendAllText(output1, "не число" + Environment.NewLine);
                    return;
                }
                else if (message == "Значение было недопустимо малым или недопустимо большим для Double.")
                {
                    File.AppendAllText(output1, "слишком много" + Environment.NewLine);
                    return;
                }
                else if (message == "Данный ключ отсутствует в словаре.")
                {
                    File.AppendAllText(output1, "неверный оператор" + Environment.NewLine);
                    return;
                }
                else if (message == "Деление на 0.")
                {
                    File.AppendAllText(output1, "bruh" + Environment.NewLine);
                    return;
                }
                else if (message == "Входная строка имела неверный формат.")
                {
                    File.AppendAllText(output1, "не парсится" + Environment.NewLine);
                    return;
                }

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

        /// <summary>
        /// Метод, который очищает файлы при повторе решения.
        /// </summary>
        public static void ClearPaths()
        {
            try
            {
                if (File.Exists(output1))
                {
                    File.Delete(output1);
                }
                if (File.Exists(output2))
                {
                    File.Delete(output2);
                }
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

        /// <summary>
        /// Метод, который крафтит файл с ответами. Считывает из файла выражения и решает их
        /// при помощи объекта-калькулятора.
        /// </summary>
        /// <param name="calc"> Калькулятор. </param>
        public static void MakeAnswerFile(Calculator calc)
        {
            try
            {
                // Считываем примеры из файла.
                string[] firstDoc = File.ReadAllLines(input1);

                // Решаем каждый пример.
                for (int i = 0; i < firstDoc.Length; i++)
                {
                    try
                    {
                        // Вычиление результата выражения из первого файла. 
                        double answer = calc.Calculate(firstDoc[i]);

                        // Запись в файл.
                        File.AppendAllText(output1, calc.Calculate(firstDoc[i]).ToString("f3") + Environment.NewLine);
                    }
                    catch (Exception)
                    {

                    }
                }
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

        /// <summary>
        /// Сверяем ответы с ответами из файла с правильными ответами. Записываем результат ещё в один файл.
        /// в случае эксепшенов, в файл записываются конкретные слова из-за обработчика события.
        /// Запишем суммарное количество ошибок.
        /// </summary>
        /// <param name="calc"> Калькулятор. </param>
        public static void CheckAnswers(Calculator calc)
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
                        if (calc.Check(answers[i], secondDoc[i]) == "Error") mistakes++; // Увелечение значения счетчика.

                        // Запись в файл.
                        File.AppendAllText(output2, calc.Check(answers[i], secondDoc[i]) + Environment.NewLine);
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
            Calculator calc = new Calculator();

            // Подписываем на событие методы-обработчики.
            calc.ErrorNotification += ConsoleErrorHandler;
            calc.ErrorNotification += ResultErrorHandler;
            do
            {
                try
                {
                    // Если программу будут перезапускать, то удалять файлы с результатами после каждого запуска.
                    Program.ClearPaths();

                    // Создаем нужные файлы.
                    MakeAnswerFile(calc);
                    CheckAnswers(calc);

                    Console.WriteLine("Чекайте файлы и подождите предложение о повторе решения. Оно что-то медлит.");
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

                // Повтор решения.
                Console.WriteLine("Нажмите Enter, чтобы повторить.");
            } while (Console.ReadKey().Key == ConsoleKey.Enter);

        }
    }
}
