﻿using CommunityToolkit.Mvvm.ComponentModel;
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
    public partial class ListsViewModel : ObservableObject
    {
        public ListsViewModel()
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

            Catalogs = new ObservableCollection<Catalog>
            {
                new Catalog
                {
                    Name = "乐器",
                    Items = new ObservableCollection<Catalog>
                    {
                        new Catalog{ Name = "钢琴" },
                        new Catalog
                        {
                            Name = "吉他",
                            Items = new ObservableCollection<Catalog>
                            {
                            new Catalog{ Name = "木吉他"},
                            new Catalog{ Name = "电吉他"}
                            }
                        },
                        new Catalog{ Name = "二胡" }
                    }
                },
                new Catalog
                {
                    Name = "运动",
                    Items = new ObservableCollection<Catalog>
                    {
                        new Catalog{ Name = "跑步" },
                        new Catalog{ Name = "篮球" },
                        new Catalog{ Name = "跑步" },
                    }
                }
            };
        }

        [ObservableProperty]
        private ObservableCollection<Person> persons;

        [ObservableProperty]
        private ObservableCollection<Catalog> catalogs;

        [ObservableProperty]
        private FoodType currentFood;
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

    public partial class Catalog : ObservableObject
    {
        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private ObservableCollection<Catalog> items;
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