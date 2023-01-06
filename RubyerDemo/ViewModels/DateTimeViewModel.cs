﻿using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubyerDemo.ViewModels
{
    public partial class DateTimeViewModel : ObservableObject
    {
        [ObservableProperty]
        private DateTime? time;

        [ObservableProperty]
        private DateTime? date;

        [ObservableProperty]
        private DateTime? dateTime;

        public DateTimeViewModel()
        {
            this.PropertyChanged += DateTimeViewModel_PropertyChanged;
        }

        private void DateTimeViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Time))
            {
                Debug.WriteLine($"TimePicker Selected: {Time}");
            }
            else if (e.PropertyName == nameof(Date))
            {
                Debug.WriteLine($"DatePicker Selected: {Date}");
            }
            else if (e.PropertyName == nameof(DateTime))
            {
                Debug.WriteLine($"DateTimePicker Selected: {DateTime}");
            }
        }
    }
}