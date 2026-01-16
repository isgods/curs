using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace curs
{
    public class TaskManager
    {
        public List<TaskItem> Tasks { get; private set; } = new();

        public static readonly Dictionary<string, int> priorityOrder = new()
        {
            { "Низкий", 1 },
            { "Средний", 2 },
            { "Высокий", 3 },
            { "Критический", 4 }
        };

        public void AddTask(TaskItem task) => Tasks.Add(task);

        public void RemoveTask(TaskItem task) => Tasks.Remove(task);

        public IEnumerable<TaskItem> FilterByStatus(TaskStatus? status)
        {
            return status == null ? Tasks : Tasks.Where(t => t.Status == status);
        }

        public IEnumerable<TaskItem> SortByDate() => Tasks.OrderBy(t => t.DueDate);

        public IEnumerable<TaskItem> SortByPriority()
        {
            return Tasks.OrderByDescending(t =>
                priorityOrder.TryGetValue(t.Priority, out int val) ? val : 0);
        }
    }
}
