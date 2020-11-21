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
    public class StepBar : ItemsControl
    {
        static StepBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(StepBar), new FrameworkPropertyMetadata(typeof(StepBar)));
        }

        protected override bool IsItemItsOwnContainerOverride(object item) => item is StepBarItem;
        protected override DependencyObject GetContainerForItemOverride() => new StepBarItem();
    }
}
