using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace RubyerDemo.ViewModels
{
    /// <summary>
    /// 滑动视图
    /// </summary>
    public partial class FlipViewViewModel : ObservableObject
    {
        [ObservableProperty]
        private List<IEasingFunction> easingFunctions;

        public FlipViewViewModel()
        {
            easingFunctions = new List<IEasingFunction>
            {
                null,
                new BackEase { EasingMode = EasingMode.EaseIn },
                new BackEase { EasingMode = EasingMode.EaseOut },
                new BackEase { EasingMode = EasingMode.EaseInOut },
                new BounceEase { EasingMode = EasingMode.EaseIn },
                new BounceEase { EasingMode = EasingMode.EaseOut },
                new BounceEase { EasingMode = EasingMode.EaseInOut },
                new CircleEase { EasingMode = EasingMode.EaseIn },
                new CircleEase { EasingMode = EasingMode.EaseOut },
                new CircleEase { EasingMode = EasingMode.EaseInOut },
                new CubicEase { EasingMode = EasingMode.EaseIn },
                new CubicEase { EasingMode = EasingMode.EaseOut },
                new CubicEase { EasingMode = EasingMode.EaseInOut },
                new ElasticEase { EasingMode = EasingMode.EaseIn },
                new ElasticEase { EasingMode = EasingMode.EaseOut },
                new ElasticEase { EasingMode = EasingMode.EaseInOut }
            };
        }
    }
}
