using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Rubyer
{
    public class MessageContainer : StackPanel
    {
        public static Dictionary<string, MessageContainer> Containers = new Dictionary<string, MessageContainer>();

        static MessageContainer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MessageContainer), new FrameworkPropertyMetadata(typeof(MessageContainer)));
        }


        public static readonly DependencyProperty IdentifyProperty =
            DependencyProperty.Register("Identify", typeof(string), typeof(MessageContainer), new PropertyMetadata(default(string), OnIdentifyChanged));

        private static void OnIdentifyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MessageContainer container = d as MessageContainer;
            string identify = e.NewValue.ToString();

            if (Containers.ContainsKey(identify))
            {
                Containers.Remove(identify);
            }

            container.Orientation = Orientation.Vertical;

            Containers.Add(identify, container);
        }

        public string Identify
        {
            get { return (string)GetValue(IdentifyProperty); }
            set { SetValue(IdentifyProperty, value); }
        }
    }
}
