using System;
using System.Diagnostics;
using System.Linq;

class Program
{
    static void Main()
    {
        while (true)
        {
            Console.WriteLine("\nМеню:");
            Console.WriteLine("1. Показать все процессы");
            Console.WriteLine("2. Найти процессы по имени");
            Console.WriteLine("3. Завершить процесс по ID");
            Console.WriteLine("4. Завершить процесс по имени");
            Console.WriteLine("0. Выйти");
            Console.Write("Выберите действие: ");

            switch (Console.ReadLine())
            {
                case "1":
                    Console.Clear();
                    ListAllProcesses();
                    break;
                case "2":
                    Console.Clear();
                    FindProcessesByName();
                    break;
                case "3":
                    Console.Clear();
                    KillProcessById();
                    break;
                case "4":
                    Console.Clear();
                    KillProcessesByName();
                    break;
                case "0":
                    return;
                default:
                    Console.Clear();
                    Console.WriteLine("Некорректный выбор, попробуйте снова.");
                    break;
            }
        }
    }

    static void ListAllProcesses()
    {
        Console.WriteLine("\nСписок процессов:");
        var currentProcessId = Process.GetCurrentProcess().Id;

        foreach (var process in Process.GetProcesses().OrderBy(p => p.ProcessName))
        {
            string marker = process.Id == currentProcessId ? " <== Это текущее приложение" : "";
            Console.WriteLine($"Имя: {process.ProcessName}, ID: {process.Id}{marker}");
        }
    }

    static void FindProcessesByName()
    {
        Console.Write("\nВведите имя процесса (или его часть): ");
        string name = Console.ReadLine()?.ToLower();

        var matchedProcesses = Process.GetProcesses()
            .Where(p => p.ProcessName.ToLower().Contains(name))
            .ToList();

        if (matchedProcesses.Any())
        {
            Console.WriteLine("\nНайденные процессы:");
            foreach (var process in matchedProcesses)
            {
                Console.WriteLine($"Имя: {process.ProcessName}, ID: {process.Id}");
            }
        }
        else
        {
            Console.WriteLine("Процессы не найдены.");
        }
    }

    static void KillProcessById()
    {
        Console.Write("\nВведите ID процесса: ");
        if (int.TryParse(Console.ReadLine(), out int processId))
        {
            try
            {
                var process = Process.GetProcessById(processId);
                
                Console.WriteLine("\nПроцес для завершения:");
                Console.WriteLine($"Имя: {process.ProcessName}, ID: {process.Id}");
                Console.Write("Вы уверены, что хотите завершить процес? (y/n): ");
                
                if (Console.ReadLine()?.ToLower() == "y")
                {
                    process.Kill();
                    process.WaitForExit();
                    Console.WriteLine("Процесс завершён.");
                }
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Процесс с таким ID не найден.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка завершения процесса: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("Некорректный ID.");
        }
    }

    static void KillProcessesByName()
    {
        Console.Write("\nВведите имя процесса: ");
        string name = Console.ReadLine()?.ToLower();

        var matchedProcesses = Process.GetProcesses()
            .Where(p => p.ProcessName.ToLower().Contains(name))
            .ToList();

        if (matchedProcesses.Any())
        {
            Console.WriteLine("\nПроцессы для завершения:");
            foreach (var process in matchedProcesses)
            {
                Console.WriteLine($"Имя: {process.ProcessName}, ID: {process.Id}");
            }

            Console.Write("Вы уверены, что хотите завершить все найденные процессы? (y/n): ");
            if (Console.ReadLine()?.ToLower() == "y")
            {
                foreach (var process in matchedProcesses)
                {
                    try
                    {
                        process.Kill();
                        process.WaitForExit();
                        Console.WriteLine($"Процесс {process.ProcessName} (ID: {process.Id}) завершён.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка завершения процесса {process.ProcessName} (ID: {process.Id}): {ex.Message}");
                    }
                }
            }
        }
        else
        {
            Console.WriteLine("Процессы не найдены.");
        }
    }
}