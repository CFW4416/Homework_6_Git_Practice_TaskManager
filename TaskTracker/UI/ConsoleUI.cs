using System;
using System.Collections.Generic;
using TaskTracker.Models;
using TaskTracker.Services;

namespace TaskTracker.UI
{
    public class ConsoleUI
    {
        private readonly TaskService taskService;
        private readonly FileStorage storage;
        private readonly bool useArrowMenu;

        public ConsoleUI(TaskService taskService, FileStorage storage, string mode)
        {
            this.taskService = taskService;
            this.storage = storage;
            this.useArrowMenu = mode.Equals("Production", StringComparison.OrdinalIgnoreCase);
        }

        public void Run()
        {
            bool running = true;
            while (running)
            {
                int choice;
                if (useArrowMenu)
                {
                    choice = Menu.ProcessArrowMenu(
                        "=== TaskTracker ===",
                        "Используйте стрелки для навигации, Enter для выбора",
                        "Показать все задачи",
                        "Добавить задачу",
                        "Изменить статус задачи",
                        "Удалить задачу",
                        "Статистика",
                        "Сохранить и выйти"
                    );
                }
                else
                {
                    ShowSimpleMenu();
                    Console.Write("Выберите пункт: ");
                    if (!int.TryParse(Console.ReadLine(), out choice)) continue;
                }

                switch (choice)
                {
                    case 1: ShowAllTasks(); break;
                    case 2: AddTask(); break;
                    case 3: ChangeTaskStatus(); break;
                    case 4: RemoveTask(); break;
                    case 5: ShowStatistics(); break;
                    case 6:
                        storage.SaveTasks(taskService.GetAllTasks());
                        running = false;
                        Console.WriteLine("Задачи сохранены. До свидания!");
                        break;
                }
            }
        }

        private void ShowSimpleMenu()
        {
            Console.WriteLine("\n=== TaskTracker ===");
            Console.WriteLine("1. Показать все задачи");
            Console.WriteLine("2. Добавить задачу");
            Console.WriteLine("3. Изменить статус задачи");
            Console.WriteLine("4. Удалить задачу");
            Console.WriteLine("5. Статистика");
            Console.WriteLine("6. Сохранить и выйти");
        }

        private void ShowAllTasks()
        {
            Console.WriteLine();
            List<TaskItem> tasks = taskService.GetAllTasks();
            if (tasks.Count == 0)
            {
                Console.WriteLine("Список задач пуст.");
            }
            else
            {
                foreach (var task in tasks)
                {
                    Console.WriteLine(task);
                }
            }
            if (useArrowMenu)
            {
                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
        }

        private void AddTask()
        {
            Console.WriteLine();
            Console.Write("Введите название задачи: ");
            string title = Console.ReadLine();
            Console.Write("Введите описание задачи: ");
            string description = Console.ReadLine();

            taskService.AddTask(title, description);
            Console.WriteLine("Задача успешно добавлена!");
            if (useArrowMenu)
            {
                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
        }

        private void ChangeTaskStatus()
        {
            Console.WriteLine();
            Console.Write("Введите ID задачи для изменения статуса: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) return;

            Console.WriteLine("1. New");
            Console.WriteLine("2. InProgress");
            Console.WriteLine("3. Done");
            Console.WriteLine("4. Cancelled");
            Console.Write("Выберите: ");
            if (!int.TryParse(Console.ReadLine(), out int statusChoice)) return;

            switch (statusChoice)
            {
                case 1: taskService.ChangeStatus(id, Models.TaskStatus.New); break;
                case 2: taskService.ChangeStatus(id, Models.TaskStatus.InProgress); break;
                case 3: taskService.ChangeStatus(id, Models.TaskStatus.Done); break;
                case 4: taskService.ChangeStatus(id, Models.TaskStatus.Cancelled); break;
            }
        }

        private void RemoveTask()
        {
            Console.WriteLine();
            List<TaskItem> tasks = taskService.GetAllTasks();
            if (tasks.Count == 0)
            {
                Console.WriteLine("Нет задач для удаления.");
                if (useArrowMenu)
                {
                    Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                }
                return;
            }

            Console.Write("Введите ID задачи для удаления: ");
            string input = Console.ReadLine();
            if (!int.TryParse(input, out int id))
            {
                Console.WriteLine("Неверный ID.");
                return;
            }

            if (taskService.RemoveTask(id))
            {
                Console.WriteLine("Задача удалена.");
            }
            else
            {
                Console.WriteLine("Задача не найдена.");
            }

            if (useArrowMenu)
            {
                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
        }

        private void ShowStatistics()
        {
            Console.WriteLine();
            Console.WriteLine("--- Статистика ---");
            Console.WriteLine(taskService.GetStatistics());

            if (useArrowMenu)
            {
                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
        }
    }
}
