using Microsoft.Extensions.DependencyInjection;
using RubyerDemo.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RubyerDemo.Views
{
    /// <summary>
    /// DateTimePickerDemo.xaml 的交互逻辑
    /// </summary>
    public partial class DateTimePickerDemo : UserControl
    {
        public DateTimePickerDemo()
        {
            InitializeComponent();

            this.DataContext = App.Current.Services.GetService<DateTimePickerViewModel>();

            calendar.BlackoutDates.Add(new CalendarDateRange(DateTime.Now));
            calendar.BlackoutDates.Add(new CalendarDateRange(DateTime.Now.AddDays(2)));
        }

        private void Clock_SelectedTimeChanged(object sender, RoutedPropertyChangedEventArgs<DateTime?> e)
        {
            Debug.WriteLine($"{e.OldValue} => {e.NewValue}");
        }

        private void TimePicker_SelectedTimeChanged(object sender, RoutedPropertyChangedEventArgs<DateTime?> e)
        {
            Debug.WriteLine($"{e.OldValue} => {e.NewValue}");
        }
    }
}
