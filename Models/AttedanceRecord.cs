using System;
using System.ComponentModel;

namespace Project_sfpgu_desktop.Models
{
    public class AttendanceRecord : INotifyPropertyChanged
    {
        public string Id { get; set; }
        public string GroupName { get; set; }
        public string StudentId { get; set; }
        public string ScheduleId { get; set; }

        private string _studentFullName;
        public string StudentFullName
        {
            get => _studentFullName;
            set
            {
                _studentFullName = value;
                OnPropertyChanged(nameof(StudentFullName));
            }
        }

        public DateTime Date { get; set; }

        private bool _isPresent;
        public bool IsPresent
        {
            get => _isPresent;
            set
            {
                if (_isPresent != value)
                {
                    _isPresent = value;
                    OnPropertyChanged(nameof(IsPresent));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
