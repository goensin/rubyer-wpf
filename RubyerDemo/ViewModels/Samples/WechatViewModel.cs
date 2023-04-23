using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubyerDemo.ViewModels.Samples
{
    public class WechatViewModel : ObservableObject
    {
        public List<WechatContact> Contacts { get; set; }
        public List<WechatMessage> Messages { get; set; }

        public WechatViewModel()
        {
            Contacts = new List<WechatContact>
            {
                new WechatContact("相亲相爱一家人", "今晚有空吗？", DateTime.Now),
                new WechatContact("订阅号信息", "广州地铁:来这个站打卡低碳出行，有惊喜~", DateTime.Now.AddHours(2).AddMinutes(3)),
                new WechatContact("微信支付", "已支付￥0.01", DateTime.Now.AddHours(4).AddMinutes(5), hasUnread:true),
            };

            Messages = new List<WechatMessage>
            {
                new WechatMessage("早上好"),
                new WechatMessage("大家好"),
                new WechatMessage("今晚有空吗?", isSelf:true),
            };
        }
    }

    public class WechatContact
    {
        public string Name { get; set; }
        public string LastMessage { get; set; }
        public DateTime LastTime { get; set; }
        public string ImageUrl { get; set; }
        public bool HasUnread { get; set; }

        public WechatContact(string name, string lastMessage, DateTime lastTime, string imageUrl = null, bool hasUnread = false)
        {
            Name = name;
            LastMessage = lastMessage;
            LastTime = lastTime;
            ImageUrl = imageUrl;
            HasUnread = hasUnread;
        }
    }

    public class WechatMessage
    {
        public string Message { get; set; }
        public bool IsSelf { get; set; }

        public WechatMessage(string message, bool isSelf = false)
        {
            Message = message;
            IsSelf = isSelf;
        }
    }
}
