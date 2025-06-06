using Project_sfpgu_desktop.Models;
using Project_sfpgu_desktop.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Project_sfpgu_desktop.ViewModels
{
    public class ScheduleViewModel : BaseViewModel
    {
        public ObservableCollection<ScheduleItem> Schedule { get; set; } = new();
        public ObservableCollection<AttendanceRecord> Attendance { get; set; } = new();
        public ObservableCollection<UserModel> Students { get; set; } = new();


        private string _groupName;
        public string GroupName
        {
            get => _groupName;
            set
            {
                _groupName = value;
                OnPropertyChanged(nameof(GroupName));
            }
        }
        private async Task LoadStudentsAndAttendanceForScheduleAsync(ScheduleItem schedule)
        {
            if (string.IsNullOrWhiteSpace(schedule.GroupName))
                return;

            Students.Clear();
            Attendance.Clear();

            var students = await ApiService.GetStudentsByGroupAsync(schedule.GroupName);
            var attendance = await ApiService.GetAttendanceByScheduleIdAsync(schedule.Id);

            if (students != null)
                foreach (var s in students)
                    Students.Add(s);

            // Сопоставляем студентов и посещаемость
            if (students != null)
            {
                foreach (var student in students)
                {
                    var record = attendance?.FirstOrDefault(a => a.StudentId == student.Id);
                    if (record == null)
                    {
                        // Если записи нет, создаём новую (можно с IsPresent = false)
                        record = new AttendanceRecord
                        {
                            StudentId = student.Id,
                            StudentFullName = student.FullName,
                            Date = schedule.Date,
                            ScheduleId = schedule.Id,
                            IsPresent = false
                        };
                    }
                    else
                    {
                        record.StudentFullName = student.FullName;
                    }
                    Attendance.Add(record);
                }
            }
        }



        public ScheduleViewModel(string userRole)
        {
            IsEditable = userRole == "admin" || userRole == "head" || userRole == "teacher" || userRole == "director";

            AddScheduleCommand = new RelayCommand(OnAddSchedule, _ => IsEditable);
            EditScheduleCommand = new RelayCommand(OnEditSchedule, _ => IsEditable && SelectedScheduleItem != null);
            DeleteScheduleCommand = new RelayCommand(OnDeleteSchedule, _ => IsEditable && SelectedScheduleItem != null);

            Attendance.CollectionChanged += (s, e) =>
            {
                if (e.NewItems != null)
                {
                    foreach (AttendanceRecord record in e.NewItems)
                    {
                        record.PropertyChanged += async (sender, args) =>
                        {
                            if (args.PropertyName == nameof(AttendanceRecord.IsPresent))
                            {
                                await UpdateAttendanceAsync(record);
                            }
                        };
                    }
                }
            };
        }

        private async void LoadSelectedScheduleDetailsAsync(ScheduleItem schedule)
        {
            try
            {
                await LoadStudentsAndAttendanceForScheduleAsync(schedule);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
            }
        }




        private ScheduleItem _selectedScheduleItem;
        public ScheduleItem SelectedScheduleItem
        {
            get => _selectedScheduleItem;
            set
            {
                _selectedScheduleItem = value;
                OnPropertyChanged(nameof(SelectedScheduleItem));

                ((RelayCommand)EditScheduleCommand).RaiseCanExecuteChanged();
                ((RelayCommand)DeleteScheduleCommand).RaiseCanExecuteChanged();

                if (value != null)
                {
                    LoadSelectedScheduleDetailsAsync(value);
                }

            }
        }


        public bool IsEditable { get; }

        public ICommand AddScheduleCommand { get; }
        public ICommand EditScheduleCommand { get; }
        public ICommand DeleteScheduleCommand { get; }


        public async Task LoadDataAsync()
        {
            if (string.IsNullOrWhiteSpace(GroupName))
            {
                MessageBox.Show("Введите название группы");
                return;
            }

            GroupName = GroupName.Trim().ToUpper();

            var scheduleItems = await ApiService.GetScheduleAsync(GroupName);
            Schedule.Clear();
            if (scheduleItems != null)
                foreach (var item in scheduleItems)
                    Schedule.Add(item);
        }

        public async Task UpdateAttendanceAsync(AttendanceRecord record)
        {
            bool result = await ApiService.UpdateAttendanceAsync(record);
            if (!result)
            {
                MessageBox.Show("Ошибка при обновлении посещаемости");
            }
        }

        private async void OnAddSchedule(object parameter)
        {
            var newItem = new ScheduleItem
            {
                Id = Guid.NewGuid().ToString(),
                GroupName = GroupName,
                Date = DateTime.Today
            };

            var editor = new Views.ScheduleItemEditor(newItem);
            if (editor.ShowDialog() == true)
            {
                bool success = await ApiService.AddScheduleItemAsync(newItem);
                if (success)
                {
                    Schedule.Add(newItem);
                }
                else
                {
                    MessageBox.Show("Ошибка при добавлении записи");
                }
            }
        }


        private async void OnEditSchedule(object parameter)
        {
            if (SelectedScheduleItem == null) return;

            var copy = new ScheduleItem
            {
                Id = SelectedScheduleItem.Id,
                GroupName = SelectedScheduleItem.GroupName,
                Subject = SelectedScheduleItem.Subject,
                TeacherFullName = SelectedScheduleItem.TeacherFullName,
                Date = SelectedScheduleItem.Date,
                Time = SelectedScheduleItem.Time
            };

            var editor = new Views.ScheduleItemEditor(copy);
            if (editor.ShowDialog() == true)
            {
                bool success = await ApiService.UpdateScheduleItemAsync(copy);
                if (success)
                {
                    SelectedScheduleItem.GroupName = copy.GroupName;
                    SelectedScheduleItem.Subject = copy.Subject;
                    SelectedScheduleItem.TeacherFullName = copy.TeacherFullName;
                    SelectedScheduleItem.Date = copy.Date;
                    SelectedScheduleItem.Time = copy.Time;

                    OnPropertyChanged(nameof(Schedule));
                }
                else
                {
                    MessageBox.Show("Ошибка при обновлении записи");
                }
            }
        }


        private async void OnDeleteSchedule(object parameter)
        {
            if (SelectedScheduleItem == null) return;

            if (!Guid.TryParse(SelectedScheduleItem.Id, out Guid scheduleId))
            {
                MessageBox.Show("Неверный формат идентификатора записи");
                return;
            }

            if (MessageBox.Show("Удалить выбранную запись?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                bool success = await ApiService.DeleteScheduleItemAsync(scheduleId);
                if (success)
                {
                    Schedule.Remove(SelectedScheduleItem);
                    SelectedScheduleItem = null;
                }
                else
                {
                    MessageBox.Show("Ошибка при удалении записи расписания");
                }
            }
        }

    }
}
