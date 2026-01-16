using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace curs
{
    public class TaskItem
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public string Priority { get; set; }

        [JsonInclude]
        public TaskStatus Status { get; set; }

        public TaskItem(string title, string description, DateTime dueDate, string priority)
        {
            Title = title;
            Description = description;
            DueDate = dueDate;
            Priority = priority;
            Status = TaskStatus.Scheduled;
        }

        public void Update(string title, string desc, DateTime due, string priority, TaskStatus status)
        {
            Title = title;
            Description = desc;
            DueDate = due;
            Priority = priority;
            Status = status;
        }

        public string StatusRus => Status switch
        {
            TaskStatus.Scheduled => "Запланировано",
            TaskStatus.InProgress => "В процессе",
            TaskStatus.Completed => "Выполнено",
            TaskStatus.Postponed => "Отложено",
            _ => ""
        };
    }
}
