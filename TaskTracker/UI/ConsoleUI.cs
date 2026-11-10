using System;
using System.Collections.Generic;
using TaskTracker.Models;
using TaskTracker.Services;

namespace TaskTracker.UI
{
    public class ConsoleUI
    {
        private TaskService taskService;
        private FileStorage storage;
        private bool useArrowMenu;

        public ConsoleUI(TaskService taskService, FileStorage storage, bool useArrowMenu)
        {
            this.taskService = taskService;
            this.storage = storage;
            this.useArrowMenu = useArrowMenu;
        }

        public void Run()
        {
            // Загружаем сохранённые задачи при старте
            List<TaskItem> saved = storage.LoadTasks();
            for (int i = 0; i < saved.Count; i++)
            {
                TaskItem t = taskService.AddTask(saved[i].Title, saved[i].Description);
                t.Status = saved[i].Status;
                t.Priority = saved[i].Priority;
            }

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
                        "Сохранить и выйти"
                    );
                }
                else
                {
                    choice = ShowSimpleMenu();
                }

                switch (choice)
                {
                    case 1:
                        ShowAllTasks();
                        break;
                    case 2:
                        AddNewTask();
                        break;
                    case 3:
                        ChangeTaskStatus();
                        break;
                    case 4:
                        RemoveTask();
                        break;
                    case 5:
                        storage.SaveTasks(taskService.GetAllTasks());
                        running = false;
                        Console.WriteLine("Задачи сохранены. До свидания!");
                        break;
                }
            }
        }

        private int ShowSimpleMenu()
        {
            Console.WriteLine();
            Console.WriteLine("=== TaskTracker ===");
            Console.WriteLine("1. Показать все задачи");
            Console.WriteLine("2. Добавить задачу");
            Console.WriteLine("3. Изменить статус задачи");
            Console.WriteLine("4. Удалить задачу");
            Console.WriteLine("5. Сохранить и выйти");
            Console.Write("Выберите действие: ");

            string input = Console.ReadLine();
            int result;
            if (int.TryParse(input, out result))
            {
                return result;
            }
            return 0;
        }

        private void ShowAllTasks()
        {
            List<TaskItem> tasks = taskService.GetAllTasks();
            Console.WriteLine();
            if (tasks.Count == 0)
            {
                Console.WriteLine("Список задач пуст.");
            }
            else
            {
                Console.WriteLine("--- Список задач ---");
                for (int i = 0; i < tasks.Count; i++)
                {
                    Console.WriteLine(tasks[i].ToString());
                }
            }

            if (useArrowMenu)
            {
                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
        }

        private void AddNewTask()
        {
            Console.WriteLine();
            Console.Write("Название задачи: ");
            string title = Console.ReadLine();

            Console.Write("Описание задачи: ");
            string description = Console.ReadLine();

            TaskItem task = taskService.AddTask(title, description);
            Console.WriteLine($"Задача добавлена: {task}");

            if (useArrowMenu)
            {
                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
        }

        private void ChangeTaskStatus()
        {
            Console.WriteLine();
            List<TaskItem> tasks = taskService.GetAllTasks();
            if (tasks.Count == 0)
            {
                Console.WriteLine("Нет задач для изменения.");
                if (useArrowMenu)
                {
                    Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                }
                return;
            }

            Console.Write("Введите ID задачи: ");
            string input = Console.ReadLine();
            int id;
            if (!int.TryParse(input, out id))
            {
                Console.WriteLine("Неверный ID.");
                return;
            }

            TaskItem task = taskService.GetTaskById(id);
            if (task == null)
            {
                Console.WriteLine("Задача не найдена.");
                return;
            }

            Console.WriteLine($"Текущий статус: {task.Status}");
            Console.WriteLine("Новый статус:");
            Console.WriteLine("1. New");
            Console.WriteLine("2. InProgress");
            Console.WriteLine("3. Done");
	    Console.WriteLine("4. Cancelled");
            Console.Write("Выберите: ");

            string statusInput = Console.ReadLine();
            int statusChoice;
            if (int.TryParse(statusInput, out statusChoice))
            {
                switch (statusChoice)
                {
                    case 1: taskService.ChangeStatus(id, TaskStatus.New); break;
                    case 2: taskService.ChangeStatus(id, TaskStatus.InProgress); break;
                    case 3: taskService.ChangeStatus(id, TaskStatus.Done); break;
	            case 4: taskService.ChangeStatus(id, TaskStatus.Cancelled); break;
                    default: Console.WriteLine("Неверный выбор."); return;
                }
                Console.WriteLine("Статус изменён.");
            }

            if (useArrowMenu)
            {
                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
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
            int id;
            if (!int.TryParse(input, out id))
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
    }
}
