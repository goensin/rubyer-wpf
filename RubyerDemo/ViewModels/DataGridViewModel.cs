using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubyerDemo.ViewModels
{
    public class DataGridViewModel : ViewModelBase
    {
        public DataGridViewModel()
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

        private ObservableCollection<Person> persons;

        public ObservableCollection<Person> Persons
        {
            get => persons;
            set
            {
                persons = value;
                RaisePropertyChanged("Persons");
            }
        }

        private RelayCommand delete;
        public RelayCommand Delete => delete ?? (delete = new RelayCommand(DeleteExecute));

        private void DeleteExecute(object obj)
        {
            var person = obj as Person;
            Persons.Remove(person);
        }
    }
}