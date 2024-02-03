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
using System.Windows.Threading;

namespace RubyerDemo.Views
{
    /// <summary>
    /// CircularPanelDemo.xaml 的交互逻辑
    /// </summary>
    public partial class CircularPanelDemo : UserControl
    {
        public CircularPanelDemo()
        {
            InitializeComponent();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            var now = DateTime.Now;
            hour.Angle = (now.Hour - 12) / 12.0 * 360;
            minutes.Angle = now.Minute / 60.0 * 360;
            second.Angle = now.Second / 60.0 * 360;
        }
    }
}
