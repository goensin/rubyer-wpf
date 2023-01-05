using System;
using System.Collections.Generic;
using System.Text;

namespace RubyerDemo.ViewModels
{
    public class MenuItem : NotifyPropertyObject
    {
        private string name;

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        private object content;
        public object Content
        {
            get => content;
            set => SetProperty(ref content, value);
        }
    }
}
