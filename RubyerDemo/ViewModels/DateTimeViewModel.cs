using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubyerDemo.ViewModels
{
    public class DateTimeViewModel : ViewModelBase
    {
        private DateTime? time;

        public DateTime? Time
        {
            get => time;
            set
            {
                SetProperty(ref time, value);
                Debug.WriteLine($"TimePicker Selected: {time}");
            }
        }

        private DateTime? date;

        public DateTime? Date
        {
            get => date;
            set
            {
                SetProperty(ref date, value);
                Debug.WriteLine($"DatePicker Selected: {date}");
            }
        }

        private DateTime? dateTime;

        public DateTime? DateTime
        {
            get => dateTime;
            set
            {
                SetProperty(ref dateTime, value);
                Debug.WriteLine($"DateTimePicker Selected: {dateTime}");
            }
        }
    }
}