using System;
using System.Collections.Generic;
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

namespace Rubyer
{
    public class MessageBoxContainer : Border
    {
        public static Dictionary<string, MessageBoxContainer> Containers = new Dictionary<string, MessageBoxContainer>();

        static MessageBoxContainer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MessageBoxContainer), new FrameworkPropertyMetadata(typeof(MessageBoxContainer)));
        }


        public static readonly DependencyProperty IdentifyProperty =
            DependencyProperty.Register("Identify", typeof(string), typeof(MessageBoxContainer), new PropertyMetadata(default(string),OnIdentifyChanged));

        private static void OnIdentifyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MessageBoxContainer container = d as MessageBoxContainer;
            string identify = e.NewValue.ToString();

            if (Containers.ContainsKey(identify))
            {
                Containers.Remove(identify);
            }

            container.Background = new SolidColorBrush(Colors.Transparent);
            container.Visibility = Visibility.Hidden;

            Containers.Add(identify, container);
        }

        public string Identify
        {
            get { return (string)GetValue(IdentifyProperty); }
            set { SetValue(IdentifyProperty, value); }
        }

    }
}
