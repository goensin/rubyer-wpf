using Rubyer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RubyerDemo.ViewModels
{
    /// <summary>
    /// The hamburger menu view model.
    /// </summary>
    public class HamburgerMenuViewModel : ViewModelBase
    {
        private string title;

        public string Title
        {
            get => title;
            set
            {
                title = value;
                RaisePropertyChanged("Title");
            }
        }

        private RelayCommand selecteCommand;

        /// <summary>
        /// 下一步命令
        /// </summary>
        public RelayCommand SelecteCommand => selecteCommand ?? (selecteCommand = new RelayCommand(SelecteExecute));

        private void SelecteExecute(object message)
        {
            Debug.WriteLine($"HamburgerMenuItem Command: {message}");
        }

        public HamburgerMenuViewModel()
        {
            Title = "hello";
        }
    }
}