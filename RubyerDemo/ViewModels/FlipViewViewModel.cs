using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace RubyerDemo.ViewModels
{
    /// <summary>
    /// 滑动视图
    /// </summary>
    public partial class FlipViewViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Image> items;

        public FlipViewViewModel()
        {
            items = new ObservableCollection<Image>();
            for (int i = 0; i < 4; i++)
            {
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(@$"../Assets/img{i + 1}.jpg", UriKind.Relative);
                bitmapImage.DecodePixelWidth = 600;
                bitmapImage.EndInit();

                items.Add(new Image
                {
                    Width = 600,
                    Source = bitmapImage
                });
            }
        }
    }
}
