# Rubyer-WPF

#### 介绍
一款自用的 WPF 主题框架，免费开源，欢迎下载点 ⭐，基本重写了系统默认控件，正在添加新控件，开发中...

#### 软件架构
基于 .Net Framework 4.5 和 .Net Core 3.1 的 WPF

#### 安装教程



#### 使用说明

在 WPF 项目的 App.Xaml 中引用:

<Application.Resources>
      <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <ResourceDictionary Source="pack://application:,,,/Rubyer;component/Themes/Generic.xaml" />
            </ResourceDictionary.MergedDictionaries>

            <Color x:Key="LightForegroundColor">#252526</Color>
            <Color x:Key="LightBackgroundColor">#FFFFFF</Color>
            <Color x:Key="DarkForegroundColor">#E6E6E6</Color>
            <Color x:Key="DarkBackgroundColor">#252526</Color>
            <SolidColorBrush x:Key="Light" Color="#6EC6FF"/>
            <SolidColorBrush x:Key="LightForeground" Color="#000000"/>
            <SolidColorBrush x:Key="Primary" Color="#2196F3"/>
            <SolidColorBrush x:Key="PrimaryForeground" Color="#FFFFFF"/>
            <SolidColorBrush x:Key="Dark" Color="#0069C0"/>
            <SolidColorBrush x:Key="DarkForeground" Color="#FFFFFF"/>
            <SolidColorBrush x:Key="Accent" Color="#F50057"/>
            <SolidColorBrush x:Key="AccentForeground" Color="#FFFFFF"/>
      </ResourceDictionary>
</Application.Resources>

#### Demo 截图

##### 按钮
![](https://gitee.com/wuyanxin1028/rubyer-wpf/raw/master/Image/Button_20201031223036.png)  

##### 输入框
![](https://gitee.com/wuyanxin1028/rubyer-wpf/raw/master/Image/InputBox_20201031223103.png)  

##### 选择框
![](https://gitee.com/wuyanxin1028/rubyer-wpf/raw/master/Image/SelectBox_20201031223126.png)  

##### 数据条
![](https://gitee.com/wuyanxin1028/rubyer-wpf/raw/master/Image/DataBar_20201031223146.png)  

##### 图标
![](https://gitee.com/wuyanxin1028/rubyer-wpf/raw/master/Image/Icon_20201031223207.png)  

##### 分组框
![](https://gitee.com/wuyanxin1028/rubyer-wpf/raw/master/Image/GroupBox_20201031223224.png)  

##### 列表与树
![](https://gitee.com/wuyanxin1028/rubyer-wpf/raw/master/Image/ListsTree_20201031223244.png)  

##### 选项卡
![](https://gitee.com/wuyanxin1028/rubyer-wpf/raw/master/Image/TabControl_20201031223318.png)  

##### 日期时间
![](https://gitee.com/wuyanxin1028/rubyer-wpf/raw/master/Image/DateTimeControl_20201031223406.png)  

##### 菜单栏
![](https://gitee.com/wuyanxin1028/rubyer-wpf/raw/master/Image/MenuBar_20201031223433.png)  

##### 文本块
![](https://gitee.com/wuyanxin1028/rubyer-wpf/raw/master/Image/TextBlock_20201031223448.png)  

##### 页码条
![](https://gitee.com/wuyanxin1028/rubyer-wpf/raw/master/Image/PageBar_20201031223509.png)  

