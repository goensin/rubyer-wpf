using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubyerDemo.ViewModels
{
    /// <summary>
    /// The step bar view model.
    /// </summary>
    public class StepBarViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StepBarViewModel"/> class.
        /// </summary>
        public StepBarViewModel()
        {
            Models = new ObservableCollection<StepModel>
            {
                new StepModel { Content = "第 1 步" , Description = DateTime.Now.ToString("HH:mm:ss") },
                new StepModel { Content = "第 2 步" , Description = DateTime.Now.AddHours(1).ToString("HH:mm:ss") },
                new StepModel { Content = "第 3 步" , Description = DateTime.Now.AddHours(2).ToString("HH:mm:ss") },
                new StepModel { Content = "第 4 步" , Description = DateTime.Now.AddHours(3).ToString("HH:mm:ss") },
                new StepModel { Content = "第 5 步" , Description = DateTime.Now.AddHours(4).ToString("HH:mm:ss") },
            };
        }

        private int currentIndex = 0;

        /// <summary>
        /// 当前步骤
        /// </summary>
        public int CurrentIndex
        {
            get => currentIndex;
            set
            {
                currentIndex = value;
                RaisePropertyChanged("CurrentIndex");
            }
        }

        private ObservableCollection<StepModel> models;

        /// <summary>
        /// 所有步骤
        /// </summary>
        public ObservableCollection<StepModel> Models
        {
            get => models;
            set
            {
                models = value;
                RaisePropertyChanged("Models");
            }
        }

        private RelayCommand nextStep;

        /// <summary>
        /// 下一步命令
        /// </summary>
        public RelayCommand NextStep => nextStep ?? (nextStep = new RelayCommand(NextStepExecute));

        private void NextStepExecute(object obj)
        {
            CurrentIndex++;
            if (CurrentIndex > 5)
            {
                CurrentIndex = 0;
            }
        }
    }

    /// <summary>
    /// 步骤模型
    /// </summary>
    public class StepModel
    {
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
    }
}