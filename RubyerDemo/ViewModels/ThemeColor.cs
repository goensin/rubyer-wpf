using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace RubyerDemo.ViewModels
{
    public class ThemeColor : NotifyPropertyObject
    {
        public string Name { get; set; }
        public Brush Primary { get; set; }
        public Brush Light { get; set; }
        public Brush Dark { get; set; }
        public Brush Accent { get; set; }
        private bool isSeleted;
        public bool IsSeleted
        {
            get => isSeleted;
            set
            {
                isSeleted = value;
                RaisePropertyChanged("IsSeleted");
            }
        }
    }
}
