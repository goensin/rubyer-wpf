using CommunityToolkit.Mvvm.ComponentModel;
using Rubyer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RubyerDemo.ViewModels
{
    public class Dialog2ViewModel : ObservableObject, IDialogDataContext
    {
        private string title;

        /// <inheritdoc/>
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        /// <inheritdoc/>
        public object CloseParameter { get; set; }

        public event Action<object> RequestClose;

        public Dialog2ViewModel()
        {
                
        }

        public void OnDialogOpened(object parameters)
        {
            Title = parameters?.ToString();
        }

        public void OnDialogClosing(RoutedEventArgs e)
        {

        }
    }
}
