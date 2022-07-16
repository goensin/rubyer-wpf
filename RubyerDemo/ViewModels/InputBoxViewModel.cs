using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubyerDemo.ViewModels
{
    public class InputBoxViewModel : ViewModelBase
    {
        private int? number;
        public int? Number
        {
            get => number;
            set
            {
                number = value;
                RaisePropertyChanged("Number");
            }
        }

        private string testpassword;
        public string TestPassword
        {
            get => testpassword;
            set
            {
                testpassword = value;
                RaisePropertyChanged("NuTestPasswordmber");
                Debug.WriteLine($"password: {testpassword}");
            }
        }

        public List<int> Heights => Enumerable.Range(0, 100).ToList();

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

        private int rangeValue = 3;
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

        private double? nullableValue;
        public double? NullableValue
        {
            get => nullableValue;
            set
            {
                nullableValue = value;
                RaisePropertyChanged("NullableIntValue");
            }
        }

        private double doubleValue = 0.1;
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

        public InputBoxViewModel()
        {
            TestPassword = "123456";
        }
    }
}
