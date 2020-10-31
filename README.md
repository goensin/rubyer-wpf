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
##### Button
![](https://gitee.com/wuyanxin1028/rubyer-wpf/raw/master/Image/Button_20201031223036.png)  

