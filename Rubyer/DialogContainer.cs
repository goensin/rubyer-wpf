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
    public class DialogContainer : Border
    {
        public static Dictionary<string, DialogContainer> containers = new Dictionary<string, DialogContainer>();

        static DialogContainer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DialogContainer), new FrameworkPropertyMetadata(typeof(DialogContainer)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }


        public static readonly DependencyProperty IdentifyProperty =
            DependencyProperty.Register("Identify", typeof(string), typeof(DialogContainer), new PropertyMetadata(default(string),OnIdentifyChanged));

        private static void OnIdentifyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DialogContainer container = d as DialogContainer;
            string identify = e.NewValue.ToString();

            if (containers.ContainsKey(identify))
            {
                containers.Remove(identify);
            }

            container.Background = new SolidColorBrush(Colors.Transparent);
            container.Visibility = Visibility.Hidden;

            containers.Add(identify, container);
        }

        public string Identify
        {
            get { return (string)GetValue(IdentifyProperty); }
            set { SetValue(IdentifyProperty, value); }
        }

    }
}
