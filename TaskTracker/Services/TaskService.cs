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

        public TaskItem GetTaskById(int id)
        {
            for (int i = 0; i < tasks.Count; i++)
            {
                if (tasks[i].Id == id)
                {
                    return tasks[i];
                }
            }
            return null;
        }

        public bool RemoveTask(int id)
        {
            TaskItem task = GetTaskById(id);
            if (task != null)
            {
                tasks.Remove(task);
                return true;
            }
            return false;
        }

        public bool ChangeStatus(int id, TaskStatus newStatus)
        {
            TaskItem task = GetTaskById(id);
            if (task != null)
            {
                task.Status = newStatus;
                return true;
            }
            return false;
        }

        public bool ChangePriority(int id, TaskPriority newPriority)
        {
            TaskItem task = GetTaskById(id);
            if (task != null)
            {
                task.Priority = newPriority;
                return true;
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

        public int GetTaskCount()
        {
            return tasks.Count;
        }
    }
}
