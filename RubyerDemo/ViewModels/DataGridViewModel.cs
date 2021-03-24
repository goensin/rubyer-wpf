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
                new Person{ Id=1,Name="张三",Age=17,IsEvolution=false,Gender=GenderType.男},
                new Person{ Id=2,Name="李四",Age=18,IsEvolution=true,Gender=GenderType.男},
                new Person{ Id=3,Name="王五",Age=19,IsEvolution=false,Gender=GenderType.女},
                new Person{ Id=4,Name="赵六",Age=20,IsEvolution=true,Gender=GenderType.男},
                new Person{ Id=5,Name="孙七",Age=21,IsEvolution=false,Gender=GenderType.男},
                new Person{ Id=6,Name="周八",Age=22,IsEvolution=true,Gender=GenderType.女},
                new Person{ Id=7,Name="吴九",Age=23,IsEvolution=false,Gender=GenderType.男}

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
