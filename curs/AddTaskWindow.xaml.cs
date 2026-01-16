using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace curs
{
    /// <summary>
    /// Логика взаимодействия для AddTaskWindow.xaml
    /// </summary>
    public partial class AddTaskWindow : Window
    {
        public TaskItem? CreatedTask { get; private set; }

        public AddTaskWindow()
        {
            InitializeComponent();
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TitleBox.Text))
            {
                MessageBox.Show("Введите название задачи!");
                return;
            }

            string priority = (PriorityBox.SelectedItem as System.Windows.Controls.ComboBoxItem)?.Content?.ToString()
                              ?? "Средний";
            DateTime due = DueBox.SelectedDate ?? DateTime.Now;

            CreatedTask = new TaskItem(
                TitleBox.Text,
                DescBox.Text,
                due,
                priority
            );

            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}

