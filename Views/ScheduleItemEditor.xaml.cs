using Project_sfpgu_desktop.Models;
using System.Windows;

namespace Project_sfpgu_desktop.Views
{
    public partial class ScheduleItemEditor : Window
    {
        public ScheduleItem ScheduleItem { get; private set; }

        public ScheduleItemEditor(ScheduleItem item)
        {
            InitializeComponent();
            ScheduleItem = item;
            DataContext = ScheduleItem;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
