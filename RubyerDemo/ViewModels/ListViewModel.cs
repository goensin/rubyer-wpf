using CommunityToolkit.Mvvm.ComponentModel;
using Rubyer.Commons;
using Rubyer.Converters;
using Rubyer.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Windows.Controls;

namespace RubyerDemo.ViewModels
{
    /// <summary>
    /// 列表视图
    /// </summary>
    public partial class ListViewModel : ObservableObject
    {
        [ObservableProperty]
        private List<string> items = new List<string>
        {
            "显示        ",
            "声音        ",
            "通知和操作  ",
            "电源和睡眠  ",
        };

        public ListViewModel()
        {
            Persons = new ObservableCollection<Person>
            {
                new Person{ Id=1,Name="张三",Age=17,IsSelected=false,Gender=GenderType.Men},
                new Person{ Id=2,Name="李四",Age=18,IsSelected=true,Gender=GenderType.Men},
                new Person{ Id=3,Name="王五",Age=19,IsSelected=false,Gender=GenderType.Women},
                new Person{ Id=4,Name="赵六",Age=20,IsSelected=true,Gender=GenderType.Men},
                new Person{ Id=5,Name="孙七",Age=21,IsSelected=false,Gender=GenderType.Men},
                new Person{ Id=6,Name="周八",Age=22,IsSelected=true,Gender=GenderType.Women},
                new Person{ Id=7,Name="吴九",Age=23,IsSelected=false,Gender=GenderType.Men}
            };
        }

        [ObservableProperty]
        private ObservableCollection<Person> persons;
    }

    public class Person : ObservableObject
    {
        private int id;

        [Display(Name = "序号", AutoGenerateField = false)]
        public int Id
        {
            get => id;
            set => SetProperty(ref id, value);
        }

        private string name;

        [Display(Name = "名称")]
        [ColumnWidth("*")]
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        private int age;

        [Display(Name = "年龄")]
        [DisplayFormat(DataFormatString = "{0}岁")]
        [ColumnWidth("120")]
        public int Age
        {
            get => age;
            set => SetProperty(ref age, value);
        }

        private bool isSelected;

        [Display(Name = "选择")]
        [ColumnWidth("80")]
        public bool IsSelected
        {
            get => isSelected;
            set => SetProperty(ref isSelected, value);
        }

        private GenderType gender;

        [Display(Name = "性别")]
        [ColumnWidth("80")]
        public GenderType Gender
        {
            get => gender;
            set => SetProperty(ref gender, value);
        }
    }

    /// <summary>
    /// 性别
    /// </summary>
    [TypeConverter(typeof(EnumDescriptionConverter))]
    public enum GenderType
    {
        [Description("男")]
        Men,

        [Description("女")]
        Women
    }
}