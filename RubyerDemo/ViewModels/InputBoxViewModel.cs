using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubyerDemo.ViewModels
{
    public class InputBoxViewModel : ViewModelBase
    {
        private int intValue;
        public int IntValue
        {
            get => intValue;
            set
            {
                intValue = value;
                RaisePropertyChanged("IntValue");
            }
        }

        private int rangeValue;
        public int RangeValue
        {
            get => rangeValue;
            set
            {
                rangeValue = value;
                RaisePropertyChanged("RangeValue");
            }
        }

        private int intervalValue;
        public int IntervalValue
        {
            get => intervalValue;
            set
            {
                intervalValue = value;
                RaisePropertyChanged("IntervalValue");
            }
        }

        private int? nullableIntValue;
        public int? NullableIntValue
        {
            get => nullableIntValue;
            set
            {
                nullableIntValue = value;
                RaisePropertyChanged("NullableIntValue");
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

        private double exponentDoubleValue;
        public double ExponentDoubleValue
        {
            get => exponentDoubleValue;
            set
            {
                exponentDoubleValue = value;
                RaisePropertyChanged("ExponentDoubleValue");
            }
        }
        
    }
}
