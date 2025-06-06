using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Project_sfpgu_desktop.Models
{
    public class ScheduleItem : INotifyPropertyChanged
    {
        private string _id;
        private string _groupName;
        private string _subject;
        private string _teacherFullName;
        private DateTime _date;
        private string _time;

        public string Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(); }
        }

        public string GroupName
        {
            get => _groupName;
            set { _groupName = value; OnPropertyChanged(); }
        }

        public string Subject
        {
            get => _subject;
            set { _subject = value; OnPropertyChanged(); }
        }

        public string TeacherFullName
        {
            get => _teacherFullName;
            set { _teacherFullName = value; OnPropertyChanged(); }
        }

        public DateTime Date
        {
            get => _date;
            set { _date = value; OnPropertyChanged(); }
        }

        public string Time
        {
            get => _time;
            set { _time = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
