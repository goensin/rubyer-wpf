using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace RubyerDemo.Views
{
    /// <summary>
    /// TextBox.xaml 的交互逻辑
    /// </summary>
    public partial class InputBox : UserControl
    {
        public InputBox()
        {
            InitializeComponent();

            DataContext = this;

            List<int> Heights = new List<int>();
            int height = 0;
            for (int i = 0; i < 30; i++)
            {
                height += 10;
                Heights.Add(height);
            }

            heightCombo.ItemsSource = Heights;

            TestPassword = "123456";
        }


        public static readonly DependencyProperty TestPasswordProperty =
            DependencyProperty.Register("TestPassword", typeof(string), typeof(InputBox), new PropertyMetadata(default(string)));

        public string TestPassword
        {
            get { return (string)GetValue(TestPasswordProperty); }
            set { SetValue(TestPasswordProperty, value); }
        }


        public int? Number
        {
            get { return (int?)GetValue(NumberProperty); }
            set { SetValue(NumberProperty, value); }
        }

        public static readonly DependencyProperty NumberProperty =
            DependencyProperty.Register("Number", typeof(int?), typeof(InputBox), new PropertyMetadata(null));

    }
}
