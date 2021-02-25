using System;
using System.Collections.Generic;
using System.Text;

namespace RubyerDemo.ViewModels
{
    public class PageBarViewModel : ViewModelBase
    {
        public PageBarViewModel()
        {
            Total = 100;
            PageSize = 1;
            //PageIndex = 1;
        }

        private int total;
        public int Total
        {
            get => total;
            set
            {
                total = value;
                RaisePropertyChanged("Total");
            }
        }

        private int pageSize;
        public int PageSize
        {
            get => pageSize;
            set
            {
                pageSize = value;
                RaisePropertyChanged("PageSize");
            }
        }

        //private int pageIndex;
        //public int PageIndex
        //{
        //    get => pageIndex;
        //    set
        //    {
        //        pageIndex = value;
        //        RaisePropertyChanged("PageIndex");
        //    }
        //}

        private string pageBarMessage;
        public string PageBarMessage
        {
            get => pageBarMessage;
            set
            {
                pageBarMessage = value;
                RaisePropertyChanged("PageBarMessage");
            }
        }



        private RelayCommand pageIndexChanged;
        public RelayCommand PageIndexChanged => pageIndexChanged ?? (pageIndexChanged = new RelayCommand(PageIndexChangedExecute));

        private void PageIndexChangedExecute(object index)
        {
            PageBarMessage = $"当前页：{(int)index}";
        }

        private RelayCommand pageSizeChanged;
        public RelayCommand PageSizeChanged => pageSizeChanged ?? (pageSizeChanged = new RelayCommand(PageSizeChangedExecute));

        private void PageSizeChangedExecute(object size)
        {
            PageBarMessage = $"每页条数：{(int)size}";
        }
    }
}
