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


        public static readonly DependencyProperty IdentifierProperty =
            DependencyProperty.Register("Identifier", typeof(string), typeof(MessageContainer), new PropertyMetadata(default(string), OnIdentifierChanged));

        private static void OnIdentifierChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
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

        public string Identifier
        {
            get { return (string)GetValue(IdentifierProperty); }
            set { SetValue(IdentifierProperty, value); }
        }
    }
}
