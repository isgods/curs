using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace curs
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TaskManager manager = new();
        private JsonTaskSaver saver = new();
        private string savePath = "tasks.json";
        private List<TaskItem> currentView = new();

        public MainWindow()
        {
            InitializeComponent();
            RefreshList();
            FilterCombo.SelectedIndex = 0;
        }

        private void RefreshList(IEnumerable<TaskItem>? list = null)
        {
            currentView = (list ?? manager.Tasks).ToList();

            TaskListBox.Items.Clear();

            foreach (var t in currentView)
                TaskListBox.Items.Add($"{t.Title} — {t.DueDate:dd.MM.yyyy} | {t.Priority} | {t.StatusRus}");

            
        }

        private TaskItem? SelectedTask => TaskListBox.SelectedIndex >= 0 ? currentView[TaskListBox.SelectedIndex] : null;

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            AddTaskWindow win = new();
            win.Owner = this;

            if (win.ShowDialog() == true && win.CreatedTask != null)
            {
                manager.AddTask(win.CreatedTask);
                RefreshList();
            }
        }

        private void RemoveTask_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedTask != null)
            {
                manager.RemoveTask(SelectedTask);
                RefreshList();
                FilterCombo.SelectedIndex = 0;
            }
        }

        private void TaskListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedTask == null) return;

            TitleBox.Text = SelectedTask.Title;
            DescBox.Text = SelectedTask.Description;
            DueBox.SelectedDate = SelectedTask.DueDate;

            PriorityBox.SelectedIndex = PriorityBox
                .Items.OfType<ComboBoxItem>()
                .Select((item, index) => new { item, index })
                .First(x => x.item.Content.ToString() == SelectedTask.Priority).index;

            StatusCombo.SelectedIndex = (int)SelectedTask.Status;
        }

        private void ApplyChanges_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedTask == null) return;

            string pr = (PriorityBox.SelectedItem as ComboBoxItem)?.Content?.ToString()
                        ?? SelectedTask.Priority;

            SelectedTask.Update(
                TitleBox.Text,
                DescBox.Text,
                DueBox.SelectedDate ?? DateTime.Now,
                pr,
                (TaskStatus)StatusCombo.SelectedIndex
            );

            RefreshList();
            FilterCombo.SelectedIndex = 0;
        }

        private void FilterCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int idx = FilterCombo.SelectedIndex;
            var filtered = idx switch
            {
                1 => manager.FilterByStatus(TaskStatus.Scheduled),
                2 => manager.FilterByStatus(TaskStatus.InProgress),
                3 => manager.FilterByStatus(TaskStatus.Completed),
                4 => manager.FilterByStatus(TaskStatus.Postponed),
                _ => manager.Tasks
            };
            RefreshList(filtered);
        }

        private void SortDate_Click(object sender, RoutedEventArgs e)
        {
            RefreshList(currentView.OrderBy(t => t.DueDate));
        }

        private void SortPriority_Click(object sender, RoutedEventArgs e)
        {
            RefreshList(currentView.OrderByDescending(t =>
                TaskManager.priorityOrder.TryGetValue(t.Priority, out int val) ? val : 0));
        }

        private void SaveTasks_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new() { Filter = "JSON File|*.json", FileName = savePath };

            if (dlg.ShowDialog() == true)
            {
                saver.Save(manager.Tasks, dlg.FileName);
                savePath = dlg.FileName;
                MessageBox.Show("Сохранено");
            }
        }

        private void LoadTasks_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new() { Filter = "JSON File|*.json" };

            if (dlg.ShowDialog() == true)
            {
                manager.Tasks.Clear();
                manager.Tasks.AddRange(saver.Load(dlg.FileName));
                savePath = dlg.FileName;
                RefreshList();
                FilterCombo.SelectedIndex = 0;
            }
        }
    }
}