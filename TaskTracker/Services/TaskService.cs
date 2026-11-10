using System;
using System.Collections.Generic;
using TaskTracker.Models;

namespace TaskTracker.Services
{
    public class TaskService
    {
        private List<TaskItem> tasks;
        private int nextId;

        public TaskService()
        {
            tasks = new List<TaskItem>();
            nextId = 1;
        }

        public TaskItem AddTask(string title, string description)
        {
            TaskItem task = new TaskItem(nextId, title, description);
            tasks.Add(task);
            nextId++;
            return task;
        }

        public List<TaskItem> GetAllTasks()
        {
            return tasks;
        }

        public bool ChangeStatus(int id, TaskStatus newStatus)
        {
            for (int i = 0; i < tasks.Count; i++)
            {
                if (tasks[i].Id == id)
                {
                    tasks[i].Status = newStatus;
                    return true;
                }
            }
            return false;
        }

        public bool RemoveTask(int id)
        {
            for (int i = 0; i < tasks.Count; i++)
            {
                if (tasks[i].Id == id)
                {
                    tasks.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        public List<TaskItem> GetTasksByStatus(TaskStatus status)
        {
            List<TaskItem> result = new List<TaskItem>();
            for (int i = 0; i < tasks.Count; i++)
            {
                if (tasks[i].Status == status)
                {
                    result.Add(tasks[i]);
                }
            }
            return result;
        }

        public string GetStatistics()
        {
            int total = tasks.Count;
            int newCount = GetTasksByStatus(TaskStatus.New).Count;
            int inProgressCount = GetTasksByStatus(TaskStatus.InProgress).Count;
            int doneCount = GetTasksByStatus(TaskStatus.Done).Count;
            int cancelledCount = GetTasksByStatus(TaskStatus.Cancelled).Count;

            return $"Всего: {total} | Новых: {newCount} | В работе: {inProgressCount} | Завершено: {doneCount} | Отменено: {cancelledCount}";
        }

        public List<TaskItem> GetTasksByPriority(TaskPriority priority)
        {
            List<TaskItem> result = new List<TaskItem>();
            for (int i = 0; i < tasks.Count; i++)
            {
                if (tasks[i].Priority == priority)
                {
                    result.Add(tasks[i]);
                }
            }
            return result;
        }
    }
}
