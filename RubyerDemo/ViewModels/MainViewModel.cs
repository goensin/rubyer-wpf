using RubyerDemo.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;

namespace RubyerDemo.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            Title = "Rubyer UI";

            MenuItems = new ObservableCollection<MenuItem>
            {
                new MenuItem{ Name="按钮-Button",Content=new Button()},
                new MenuItem{ Name="输入框-InputBox",Content=new InputBox() },
                new MenuItem{ Name="选择框-SelectBox",Content=new SelectBox()},
                new MenuItem{ Name="数据条-DataBar",Content=new DataBar()},
                new MenuItem{ Name="图标-Icon",Content=new Icon{ DataContext = new IconViewModel()} },
                new MenuItem{ Name="分组框-GroupBox",Content=new GroupBox() },
                new MenuItem{ Name="列表与树-ListsTree",Content=new ListsTree{ DataContext = new ListsViewModel()} },
                new MenuItem{ Name="选项卡-TabControl",Content=new TabControl{ DataContext = new TabControlViewModel()} },
                new MenuItem{ Name="日期时间-DateTimeControl",Content=new DateTimeControl{} },
                new MenuItem{ Name="菜单栏-MenuBar",Content=new MenuBar{} },
                new MenuItem{ Name="文本块-TextBlock",Content=new TextBlock{} },
                new MenuItem{ Name="页码条-PageBar",Content=new PageBar{ DataContext = new PageBarViewModel()} }
            };

            CurrentMenuItem = MenuItems[0];
        }

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

        private ObservableCollection<MenuItem> menuItems;
        public ObservableCollection<MenuItem> MenuItems
        {
            get => menuItems;
            set
            {
                menuItems = value;
                RaisePropertyChanged("MenuItems");
            }
        }

        private MenuItem currentMenuItem;
        public MenuItem CurrentMenuItem
        {
            get => currentMenuItem;
            set
            {
                currentMenuItem = value;
                RaisePropertyChanged("CurrentMenuItem");
            }
        }



        private RelayCommand hello;
        public RelayCommand Hello => hello ?? (hello = new RelayCommand(HelloExecute));

        private void HelloExecute(object obj)
        {
            MessageBox.Show("Hello");
        }
    }
}
