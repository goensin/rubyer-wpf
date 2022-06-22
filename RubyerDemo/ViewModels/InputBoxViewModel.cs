using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubyerDemo.ViewModels
{
    public class InputBoxViewModel : ViewModelBase
    {
        private double intValue;
        public double IntValue
        {
            get => intValue;
            set
            {
                intValue = value;
                RaisePropertyChanged("IntValue");
            }
        }

        private double rangeValue;
        public double RangeValue
        {
            get => rangeValue;
            set
            {
                rangeValue = value;
                RaisePropertyChanged("RangeValue");
            }
        }

        private double intervalValue;
        public double IntervalValue
        {
            get => intervalValue;
            set
            {
                intervalValue = value;
                RaisePropertyChanged("IntervalValue");
            }
        }

        private double doubleValue;
        public double DoubleValue
        {
            get => doubleValue;
            set
            {
                doubleValue = value;
                RaisePropertyChanged("DoubleValue");
            }
        }
    }
}
