# Rubyer-WPF

#### 介绍
一款自用的 WPF 主题和控件包，免费开源，欢迎下载点 ⭐，基本重写了系统默认控件；
基本重写了 WPF 默认控件的样式，增加常用控件，考虑包大小，只做 bug 修复，不增加新控件了，更多自定义控件考虑拓展包形式。

#### 软件架构
基于 .Net Framework 4.6 和 .Net Core 3.1 的 WPF 

#### 安装教程
Install-Package Rubyer 添加引用，
或者 Nuget 搜索 Rubyer 安装。

#### 使用说明

在 WPF 项目的 App.Xaml 中引用:

```
<Application.Resources>
      <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <ResourceDictionary Source="pack://application:,,,/Rubyer;component/Themes/Generic.xaml" />
            </ResourceDictionary.MergedDictionaries>
      </ResourceDictionary>
</Application.Resources>
```

#### Demo 截图

##### 按钮
<img src="https://gitee.com/wuyanxin1028/rubyer-wpf/raw/master/Image/Button.png" height="400"/><br/> 

##### 输入框
<img src="https://gitee.com/wuyanxin1028/rubyer-wpf/raw/master/Image/InputBox.png" height="400"/><br/> 

##### 选择框
<img src="https://gitee.com/wuyanxin1028/rubyer-wpf/raw/master/Image/SelectBox.png" height="400"/><br/>  

##### 数据条
<img src="https://gitee.com/wuyanxin1028/rubyer-wpf/raw/master/Image/DataBar.png" height="400"/><br/> 

##### 图标
<img src="https://gitee.com/wuyanxin1028/rubyer-wpf/raw/master/Image/Icon.png" height="400"/><br/> 

##### 分组框
<img src="https://gitee.com/wuyanxin1028/rubyer-wpf/raw/master/Image/GroupBox.png" height="400"/><br/> 

##### 列表与树
<img src="https://gitee.com/wuyanxin1028/rubyer-wpf/raw/master/Image/ListsTree.png" height="400"/><br/> 

##### 选项卡
<img src="https://gitee.com/wuyanxin1028/rubyer-wpf/raw/master/Image/TabControl.png" height="400"/><br/> 

##### 日期时间
<img src="https://gitee.com/wuyanxin1028/rubyer-wpf/raw/master/Image/DateTimeControl.png" height="400"/><br/> 

##### 菜单栏
<img src="https://gitee.com/wuyanxin1028/rubyer-wpf/raw/master/Image/MenuBar.png" height="400"/><br/> 

##### 文本块
<img src="https://gitee.com/wuyanxin1028/rubyer-wpf/raw/master/Image/TextBlock.png" height="400"/><br/> 

##### 页码条
<img src="https://gitee.com/wuyanxin1028/rubyer-wpf/raw/master/Image/PageBar.png" height="400"/><br/> 

##### 消息提示
<img src="https://gitee.com/wuyanxin1028/rubyer-wpf/raw/master/Image/MessageInfo.png" height="400"/><br/> 

##### 消息框
<img src="https://gitee.com/wuyanxin1028/rubyer-wpf/raw/master/Image/MessageBox.png" height="400"/><br/> 

##### 对话框
<img src="https://gitee.com/wuyanxin1028/rubyer-wpf/raw/master/Image/DialogBox.png" height="400"/><br/> 
