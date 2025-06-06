using System.Windows;
using System.Windows.Controls;
using Project_sfpgu_desktop.ViewModels;

namespace Project_sfpgu_desktop.Views
{
    public partial class SchedulePage : UserControl
    {
        private ScheduleViewModel _vm;

        public SchedulePage(string userRole)
        {
            InitializeComponent();
            _vm = new ScheduleViewModel(userRole);
            DataContext = _vm;
        }

        private async void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            await _vm.LoadDataAsync();
        }
    }

}
