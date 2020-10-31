
using System.Windows.Media;

namespace Rubyer
{
    public class PageInfo
    {
        public string Name { get; set; }
        public string ToolTip { get; set; }
        public int Value { get; set; }
        public bool IsEnabled { get; set; }
        public RelayCommand IndexChangeCommand { get; set; }
        public Brush Background { get; set; }
        public Brush Foreground { get; set; }
    }
}
